﻿using AuctionRoomService.Data;
using AuctionRoomService.Entities.Enums;
using KernelShared;
using MassTransit;

namespace AuctionRoomService.Features.Customers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly AuctionDbContext _context;

        public AuctionFinishedConsumer(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            // Get the auctions that are finished
            var auction = await _context.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

            if (context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.AmountSold = context.Message.Amount;
            }

            auction.Status = auction.AmountSold > auction.ReservedPrice
                ? Status.Finished
                : Status.ReserveNotMet;

            await _context.SaveChangesAsync();
        }
    }
}
