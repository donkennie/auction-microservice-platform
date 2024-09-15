using AuctionRoomService.DTOs;
using AuctionRoomService.Services;
using AutoMapper;
using MediatR;
using System.Net;

namespace AuctionRoomService.Features.Commands
{
    public class DeleteAuctionCommand : IRequest<APIResponse>
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public sealed class Handler : IRequestHandler<DeleteAuctionCommand, APIResponse>
        {
            private readonly IAuctionRoomService _auctionService;
            private readonly IMapper _mapper;

            public Handler(IAuctionRoomService auctionService, IMapper mapper)
            {
                _auctionService = auctionService;
                _mapper = mapper;
            }

            public async Task<APIResponse> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
            {
                var auction = await _auctionService.GetAuctionEntityById(request.Id);
                if (auction is null)
                    return APIResponse.GetFailureMessage(HttpStatusCode.BadRequest, null, "Auction is not found");

                if (auction.Seller != request.UserId)
                {
                    return APIResponse.GetFailureMessage(HttpStatusCode.BadRequest, null, "User wrong resource");
                }
                _auctionService.DeleteAuction(auction);
                 await _auctionService.SaveChangesAsync();

                return APIResponse.GetSuccessMessage(HttpStatusCode.Created, data: null, "Deleted Successfully!");
            }
        }
    }
}
