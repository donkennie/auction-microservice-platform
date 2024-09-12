using AuctionRoomService.Entities.Enums;

namespace AuctionRoomService.Entities
{
    public class Auction : BaseEntity
    {
        public int ReservePrice { get; set; }
        public string Seller { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public decimal AmountSold { get; set; }
        public int CurrentHighBid { get; set; }
        public DateTime EndTime { get; set; }
        public Status Status { get; set; }
        public Item Item { get; set; }

        public bool HasReservePrice() => ReservePrice > 0;

    }

}
