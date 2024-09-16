using BiddingService.Entities.Enums;
using MongoDB.Entities;

namespace BiddingService.Entities
{
    public class Auction: Entity
    {
        public DateTime AuctionEnd { get; set; }
        public string Seller { get; set; }
        public int ReservePrice { get; set; }
        public AuctionStatus Status { get; set; }
    }
}
