namespace AuctionRoomService.DTOs
{
    public record AuctionDTO
    {
        public Guid Id { get; set; }
        public int ReservedPrice { get; set; }
        public string Seller { get; set; }
        public string Winner { get; set; }
        public decimal AmountSold { get; set; }
        public int CurrentHighBid { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime EndTime { get; set; }
    }
}
