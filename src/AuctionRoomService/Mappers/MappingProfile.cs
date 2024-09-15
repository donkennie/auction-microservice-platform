using AuctionRoomService.DTOs;
using AuctionRoomService.Entities;
using AuctionRoomService.Features.Commands;
using AutoMapper;

namespace AuctionRoomService.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Auction, AuctionDTO>().ReverseMap();
            CreateMap<CreateAuctionCommand, Auction>().ReverseMap();
        }
    }
}
