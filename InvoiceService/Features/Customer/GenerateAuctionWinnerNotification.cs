using AutoMapper;
using InvoiceService.Data;
using InvoiceService.Entities;
using KernelShared;
using MassTransit;

namespace InvoiceService.Features.Customer

{
    public class GenerateAuctionWinnerNotification : IConsumer<AuctionWinnerNotified>
    {
        private readonly InvoiceDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public GenerateAuctionWinnerNotification(InvoiceDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<AuctionWinnerNotified> context)
        {
            Console.WriteLine("--> Consuming Auction Winner Notified event");

            var auctionWinnerNotified = context.Message;

            var invoice = _mapper.Map<Invoice>(auctionWinnerNotified);

            invoice.Id = Guid.NewGuid(); 

            await _dbContext.Invoices.AddAsync(invoice);
            await _dbContext.SaveChangesAsync();

            var invoiceGeneratedEvent = _mapper.Map<InvoiceGenerated>(invoice);

            await _publishEndpoint.Publish(invoiceGeneratedEvent);

            Console.WriteLine($"--> InvoiceGenerated event published for Invoice ID: {invoice.Id}");
        }
    }
}
