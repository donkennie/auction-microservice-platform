using AuctionRoomService.DTOs;
using AuctionRoomService.Services;
using AutoMapper;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AuctionRoomService.Features.Queries
{
    public class GetAuctionByDateQuery : IRequest<List<APIResponse>>
    {
        public string Date { get; set; }

        public class GetAuctionByDateQueryHandler : IRequestHandler<GetAuctionByDateQuery, List<APIResponse>>
        {
            private readonly IAuctionRoomService _auctionRoomService;
            private readonly IMapper _mapper;

            public GetAuctionByDateQueryHandler(IAuctionRoomService auctionRoomService, IMapper mapper)
            {
                _auctionRoomService = auctionRoomService;
                _mapper = mapper;
            }

            public async Task<List<APIResponse>> Handle(GetAuctionByDateQuery request, CancellationToken cancellationToken)
            {

                var auctions = await _auctionRoomService.GetAuctionsAsync(request.Date);

                if (auctions == null || !auctions.Any())
                {
                    return new List<APIResponse>();
                }

                return _mapper.Map<List<APIResponse>>(auctions);
            }
        }
    }
}