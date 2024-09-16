using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Entities;
using MediatR;
using MongoDB.Entities;

namespace BiddingService.Features.Queries
{
    public class GetBidsForAuctionQuery : IRequest<List<APIResponse>>
    {
        public string AuctionId { get; init; }

        public class GetBidsForAuctionQueryHandler : IRequestHandler<GetBidsForAuctionQuery, List<APIResponse>>
        {
            private readonly IMapper _mapper;

            public GetBidsForAuctionQueryHandler(IMapper mapper)
            {
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<List<APIResponse>> Handle(GetBidsForAuctionQuery request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.AuctionId))
                {
                    throw new ArgumentException("AuctionId cannot be null or empty", nameof(request.AuctionId));
                }

                var bids = await DB.Find<Bid>()
                    .Match(a => a.AuctionId == request.AuctionId)
                    .Sort(b => b.Descending(a => a.BidTime))
                    .ExecuteAsync(cancellationToken);

                if (bids == null || bids.Count == 0)
                {
                    return new List<APIResponse>(); // Return an empty list if no bids are found
                }

                return _mapper.Map<List<APIResponse>>(bids);
            }
        }
    } 
}
