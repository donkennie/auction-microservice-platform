﻿using AuctionRoomService.Data;
using KernelShared;
using MassTransit;

namespace AuctionRoomService.Features.Customers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _context;

        public BidPlacedConsumer(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            var auction = await _context.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

            if (auction.CurrentHighBid == null
                || context.Message.BidStatus.Contains("Accepted")
                && context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _context.SaveChangesAsync();
            }
        }
    }
}
