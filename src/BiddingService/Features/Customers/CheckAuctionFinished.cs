﻿using BiddingService.Entities;
using BiddingService.Entities.Enums;
using KernelShared;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService.Features.Customers
{
    public class CheckAuctionFinished : BackgroundService
    {
        private readonly ILogger<CheckAuctionFinished> _logger;
        private readonly IServiceProvider _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckAuctionFinished"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging information and errors.</param>
        /// <param name="services">The service provider used for creating service scopes.</param>
        public CheckAuctionFinished(ILogger<CheckAuctionFinished> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        /// <summary>
        /// Executes the background task to check for finished auctions. This task runs continuously
        /// with a delay between each execution cycle.
        /// </summary>
        /// <param name="stoppingToken">A <see cref="CancellationToken"/> used to signal the task to stop.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting check for finished auctions");

            stoppingToken.Register(() => _logger.LogInformation("==> Auction check is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAuctions(stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }

        /// <summary>
        /// Checks for auctions that have ended but not marked as finished, updates their status,
        /// and publishes a message indicating the auction has finished.
        /// </summary>
        /// <param name="stoppingToken">A <see cref="CancellationToken"/> used to signal the operation to stop.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous check operation.</returns>
        private async Task CheckAuctions(CancellationToken stoppingToken)
        {
            var finishedAuctions = await DB.Find<Auction>()
                .Match(x => x.AuctionEnd <= DateTime.UtcNow)
                .Match(x => x.Status != AuctionStatus.Finished)
                .ExecuteAsync(stoppingToken);

            if (finishedAuctions.Count == 0) return;

            _logger.LogInformation("==> Found {Count} auctions that have completed", finishedAuctions.Count);

            using var scope = _services.CreateScope();
            var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            foreach (var auction in finishedAuctions)
            {
                auction.Status = AuctionStatus.Finished;
                await auction.SaveAsync(null, stoppingToken);

                var winningBid = await DB.Find<Bid>()
                    .Match(a => a.AuctionId == auction.ID)
                    .Match(b => b.BidStatus == BidStatus.Accepted)
                    .Sort(x => x.Descending(s => s.Amount))
                    .ExecuteFirstAsync(stoppingToken);

                await endpoint.Publish(new AuctionFinished
                {
                    ItemSold = winningBid != null,
                    AuctionId = auction.ID,
                    Winner = winningBid.Bidder,
                    Amount = winningBid.Amount,
                    Seller = auction.Seller,
                    AuctionCompletionDate = DateTime.Now,
                    WinningBidAmount = winningBid?.Amount,
                    InvoiceDate = DateTime.Now,
                    HighestBidder = new BidderInfo(winningBid?.Bidder, winningBid?.Bidder, winningBid?.Bidder),
                    ItemDetails = new AuctionItemDetails(winningBid.ID, "", "")
                }, stoppingToken);
            }
        }
    }
}
