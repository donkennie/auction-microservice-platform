using AuctionRoomService.DTOs;
using AuctionRoomService.Entities.Enums;

namespace AuctionRoomService.Entities
{
    public class Auction : BaseEntity
    {
        public int ReservedPrice { get; set; }
        public string Seller { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public decimal AmountSold { get; set; }
        public int CurrentHighBid { get; set; }
        public DateTime EndTime { get; set; }
        public Status Status { get; set; }
        public virtual Item Item { get; set; }

        public bool HasReservePrice() => ReservedPrice > 0;

        public void Update(ItemDTO itemDTO)
        {
            this.Item.Color = itemDTO.Color;
            this.Item.Make = itemDTO.Make;
            this.Item.Year = (int)itemDTO.Year;
            this.Item.Mileage = itemDTO.Mileage;
            this.Item.Model = itemDTO.Model;
        }

    }

}
