using AuctionRoomService.Data;
using AuctionRoomService.DTOs;
using AuctionRoomService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuctionRoomService.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;

        public AuctionService(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateAuction(Auction? auction) =>
            _context.Auctions.Add(auction);

        public async Task<AuctionDTO?> GetAuctionByIdAsync(Guid id)
        {
            return await _context.Auctions
                                 .ProjectTo<AuctionDTO>(_mapper.ConfigurationProvider)
                                 .FirstOrDefaultAsync(auction => auction.Id == id);
        }

        public async Task<Auction?> GetAuctionEntityById(Guid id)
        {
            return await _context.Auctions.Include(auction => auction.Item)
                                 .FirstOrDefaultAsync(auction => auction.Id == id);
        }

        public async Task<List<AuctionDTO>> GetAuctionsAsync(string? date)
        {
            var query = _context.Auctions.OrderBy(auction => auction.Item.Make).AsQueryable();

            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.LastModified.CompareTo(DateTime.Parse(date)
                                                                   .ToUniversalTime())
                                       > 0);
            }

            return await query.ProjectTo<AuctionDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public void DeleteAuction(Auction? auction) =>
            _context.Auctions.Remove(auction);

        public async Task<bool> SaveChangesAsync() =>
             await _context.SaveChangesAsync() > 0;

    }
}
