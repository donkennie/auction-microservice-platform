namespace InvoiceService.Entities
{
    public class Invoice : BaseEntity
    {
        public string AuctionId { get; set; } = string.Empty;
        public AuctionItemDetails ItemDetails { get; set; }
        public BidderDetails TopBidder { get; set; }
        public decimal WinningBidAmount { get; set; }
        public DateTime AuctionCompletionDate { get; set; }
        public string BillingAddress { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string PaymentInstructions { get; set; } = string.Empty;
        public string RefundPolicy { get; set; } = string.Empty;
    }
}
