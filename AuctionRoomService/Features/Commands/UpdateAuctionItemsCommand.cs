using AuctionRoomService.DTOs;
using AuctionRoomService.Services;
using AutoMapper;
using MediatR;
using System.Net;

namespace AuctionRoomService.Features.Commands
{
    public class UpdateAuctionItemsCommand : IRequest<APIResponse>
    {
        public Guid Id { get; set; }
        public ItemDTO ItemDTO { get; set; }
        public sealed class Handler : IRequestHandler<UpdateAuctionItemsCommand, APIResponse>
        {
            private readonly IAuctionService _auctionService;

            public Handler(IAuctionService auctionService, IMapper mapper) =>
                _auctionService = auctionService;


            public async Task<APIResponse> Handle(UpdateAuctionItemsCommand request, CancellationToken cancellationToken)
            {
                var auction = await _auctionService.GetAuctionEntityById(request.Id);
                if (auction is null)
                    return APIResponse.GetFailureMessage(HttpStatusCode.BadRequest, null, "Auction is not found");

                auction.Update(request.ItemDTO);
                _auctionService.DeleteAuction(auction);
                await _auctionService.SaveChangesAsync();

                return APIResponse.GetSuccessMessage(HttpStatusCode.Created, data: null, "Updated Successfully!");
            }
        }
    }
}
