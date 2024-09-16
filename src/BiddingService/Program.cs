using BiddingService.Data;
using BiddingService.Features.Commands;
using BiddingService.Features.Customers;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer()
       .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(BidCommand).GetTypeInfo().Assembly))

       .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
       .AddControllers();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseRetry(r =>
        {
            r.Handle<RabbitMqConnectionException>();
            r.Interval(5, TimeSpan.FromSeconds(10));
        });
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ConfigureEndpoints(context);
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await Policy.Handle<TimeoutException>().WaitAndRetryAsync(5, t => TimeSpan.FromSeconds(10))
    .ExecuteAndCaptureAsync(async () =>
    {
        await DB.InitAsync("BidDb",
            MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("BidDbConnection")));
        await DbInitializer.InitDb(app);
    });

app.Run();
