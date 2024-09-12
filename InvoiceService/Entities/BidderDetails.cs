namespace InvoiceService.Entities
{
    public class BidderDetails : BaseEntity
    {
        public string BidderId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public BidderDetails()
        {
            Id = Guid.NewGuid();
        }
    }
}
