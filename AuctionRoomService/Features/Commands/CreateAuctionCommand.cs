using AuctionRoomService.DTOs;
using AuctionRoomService.Entities;
using AuctionRoomService.Entities.Enums;
using AuctionRoomService.Services;
using AutoMapper;
using MediatR;
using System.Net;

namespace AuctionRoomService.Features.Commands
{
    public class CreateAuctionCommand: IRequest<APIResponse>
    {
        public Guid Id { get; set; }
        public int ReservePrice { get; set; }
        public string Seller { get; set; }
        public string Winner { get; set; }
        public decimal? SoldAmount { get; set; }
        public int? CurrentHighBid { get; set; }
        public DateTime AuctionEnd { get; set; }
        public Status Status { get; set; }
        public Item Item { get; set; }

        public sealed class Handler : IRequestHandler<CreateAuctionCommand, APIResponse>
        {
            private readonly IAuctionService _auctionService;
            private readonly IMapper _mapper;

            public Handler(IAuctionService auctionService, IMapper mapper)
            {
                _auctionService = auctionService;
                _mapper = mapper;
            }

            public async Task<APIResponse> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
            {
                var data = _mapper.Map<Auction>(request);

                _auctionService.CreateAuction(data);
                var saveChanges = await _auctionService.SaveChangesAsync();
                if(!saveChanges)
                    return APIResponse.GetFailureMessage(HttpStatusCode.BadRequest, null, "Failed to create Auction");

                return APIResponse.GetSuccessMessage(HttpStatusCode.Created, data: null, "Created Successfully!");
            }
        }
    }
}
