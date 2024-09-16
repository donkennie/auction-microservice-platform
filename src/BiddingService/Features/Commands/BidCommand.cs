using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Entities;
using BiddingService.Entities.Enums;
using KernelShared;
using MassTransit;
using MediatR;
using MongoDB.Entities;

namespace BiddingService.Features.Commands
{
    public class BidCommand: IRequest<APIResponse>
    {
        public string AuctionId { get; init; }
        public int Amount { get; init; }
        public string Bidder { get; init; }


        public class BidCommandHandler : IRequestHandler<BidCommand, APIResponse>
        {
            private readonly IMapper _mapper;
            private readonly IPublishEndpoint _publishEndpoint;

            public BidCommandHandler(IMapper mapper, IPublishEndpoint publishEndpoint)
            {
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            }

            public async Task<APIResponse> Handle(BidCommand request, CancellationToken cancellationToken)
            {
                ValidateRequest(request);

                var auction = await DB.Find<Auction>()
                                      .OneAsync(request.AuctionId, cancellationToken);

                if (auction == null)
                    throw new BadRequestException("Auction not found");

                ValidateAuctionState(auction, request);

                var bid = CreateBid(request, auction);
                SetBidStatus(bid, auction, cancellationToken);

                await SaveBidAsync(bid);
                await PublishBidPlacedEventAsync(bid, cancellationToken);

                return _mapper.Map<APIResponse>(bid);
            }

            #region Private Methods
            private void ValidateRequest(BidCommand request)
            {
                if (string.IsNullOrEmpty(request.AuctionId))
                    throw new ArgumentException("AuctionId cannot be null or empty", nameof(request.AuctionId));

                if (string.IsNullOrEmpty(request.Bidder))
                    throw new ArgumentException("Bidder cannot be null or empty", nameof(request.Bidder));

                if (request.Amount <= 0)
                    throw new ArgumentException("Bid amount must be greater than zero", nameof(request.Amount));
            }

            private void ValidateAuctionState(Auction auction, BidCommand request)
            {
                if (auction.Seller == request.Bidder)
                    throw new BadRequestException("You cannot bid on your own auction");

                if (auction.AuctionEnd < DateTime.UtcNow)
                    throw new BadRequestException("Cannot accept bids on this auction at this time");
            }

            private Bid CreateBid(BidCommand request, Auction auction)
            {
                return new Bid
                {
                    Amount = request.Amount,
                    AuctionId = request.AuctionId,
                    Bidder = request.Bidder,
                    BidStatus = auction.AuctionEnd < DateTime.UtcNow ? BidStatus.Finished : BidStatus.TooLow,
                    BidTime = DateTime.UtcNow
                };
            }

            private async void SetBidStatus(Bid bid, Auction auction, CancellationToken cancellationToken)
            {
                var highBid = await DB.Find<Bid>()
                     .Match(a => a.AuctionId == bid.AuctionId)
                     .Sort(b => b.Descending(x => x.Amount))
                     .ExecuteFirstAsync();

                if (highBid == null || bid.Amount > highBid.Amount)
                {
                    bid.BidStatus = bid.Amount > auction.ReservePrice
                        ? BidStatus.Accepted
                        : BidStatus.AcceptedBelowReserve;
                }
                else
                {
                    bid.BidStatus = BidStatus.TooLow;
                }
            }

            private async Task SaveBidAsync(Bid bid)
            {
                await DB.SaveAsync(bid);
            }

            private async Task PublishBidPlacedEventAsync(Bid bid, CancellationToken cancellationToken)
            {
                var bidPlacedEvent = _mapper.Map<BidPlaced>(bid);
                await _publishEndpoint.Publish(bidPlacedEvent, cancellationToken);
            }
            #endregion
        }
    }
}
