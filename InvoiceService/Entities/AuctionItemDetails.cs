namespace InvoiceService.Entities
{
    public class AuctionItemDetails : BaseEntity
    {
        public string ItemId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public AuctionItemDetails()
        {
            Id = Guid.NewGuid();
        }
    }
}
