namespace KernelShared
{
    public class AuctionFinished
    {
        public bool ItemSold { get; set; }
        public BidderInfo HighestBidder { get; set; }
        public bool IsItemSold { get; set; } = false;
        public decimal? WinningBidAmount { get; set; }
        public AuctionItemDetails ItemDetails { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime AuctionCompletionDate { get; set; }
        public string AuctionId { get; set; }
        public string Winner { get; set; }
        public string Seller { get; set; }
        public decimal Amount { get; set; }
    }
}
