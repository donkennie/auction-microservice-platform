using AuctionRoomService.DTOs;
using AuctionRoomService.Services;
using AutoMapper;
using MediatR;
using System.Net;

namespace AuctionRoomService.Features.Queries
{
    public class GetAuctionsQuery : IRequest<APIResponse>
    {
        public Guid Id { get; set; }
        public sealed class Handler : IRequestHandler<GetAuctionsQuery, APIResponse>
        {
            private readonly IAuctionService _auctionService;

            public Handler(IAuctionService auctionService, IMapper mapper) =>
                _auctionService = auctionService;

            public async Task<APIResponse> Handle(GetAuctionsQuery request, CancellationToken cancellationToken)
            {
                var auctions = await _auctionService.GetAuctionByIdAsync(request.Id);
                if (auctions is null)
                    return APIResponse.GetFailureMessage(HttpStatusCode.BadRequest, null, "Auction is not found");

                return APIResponse.GetSuccessMessage(HttpStatusCode.Created, data: auctions, "Fetched Successfully!");
            }
        }
   
    }
}
