namespace KernelShared
{
    public class AuctionWinnerNotified(string auctionId, AuctionItemDetails itemDetails, BidderInfo highestBidder,
            decimal winningBidAmount, DateTime completedAt,
            string billingAddress, DateTime invoiceDate, string paymentInstructions, string refundPolicy)
    {
     
    }

    public class AuctionItemDetails(string itemId, string name, string description) {
    }

    public class BidderInfo(string bidderId, string fullName, string email){
    }

}

