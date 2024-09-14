using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelShared
{
    public class InvoiceGenerated
    {
        public Guid Id { get; set; }
        public string AuctionId { get; set; }
        public AuctionItemDetails ItemDetails { get; set; }
        public BidderInfo HighestBidder { get; set; }
        public double WinningBidAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BillingAddress { get; set; }
        public string RefundPolicy { get; set; }
        public decimal TaxesAndFees { get; set; }
    }
}
