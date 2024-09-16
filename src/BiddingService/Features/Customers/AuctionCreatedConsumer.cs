﻿using BiddingService.Entities;
using BiddingService.Entities.Enums;
using KernelShared;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService.Features.Customers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            var auction = new Auction
            {
                ID = context.Message.Id.ToString(),
                Seller = context.Message.Seller,
                AuctionEnd = context.Message.AuctionEnd,
                ReservePrice = context.Message.ReservePrice,
                Status = AuctionStatus.Active
            };

            await auction.SaveAsync();
        }
    }
}
