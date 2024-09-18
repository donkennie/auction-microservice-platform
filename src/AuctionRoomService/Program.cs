

using AuctionRoomService.Data;
using AuctionRoomService.Features.Commands;
using AuctionRoomService.Services;
using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
       .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAuctionCommand).GetTypeInfo().Assembly))
       
       .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
       .AddControllers();

builder.Services.AddDbContext<AuctionDbContext>(Options =>
{
    Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

});

// Add services to the container.
builder.Services.AddScoped<IAuctionRoomService, RoomService>();

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

try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
