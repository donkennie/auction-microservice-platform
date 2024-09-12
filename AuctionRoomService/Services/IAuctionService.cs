using AuctionRoomService.DTOs;
using AuctionRoomService.Entities;

namespace AuctionRoomService.Services
{
    public interface IAuctionService
    {
        Task<List<AuctionDTO>> GetAuctionsAsync(string? date);
        Task<AuctionDTO?> GetAuctionByIdAsync(Guid id);
        Task<Auction?> GetAuctionEntityById(Guid id);
        void CreateAuction(Auction? auction);
        void DeleteAuction(Auction? auction);
        Task<bool> SaveChangesAsync();
    }
}
