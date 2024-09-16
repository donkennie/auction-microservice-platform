using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Entities;
using KernelShared;

namespace BiddingService.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Bid,BidDTO>();
            CreateMap<Bid, BidPlaced>();
        }
    }
}
