

using AuctionRoomService.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
       .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
       //.AddCustomDbContext(builder.Configuration)
       //.AddCustomAuthentication(builder.Configuration)
       //.AddCustomServices()
       .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
      // .AddCustomIntegrationTransport(builder.Configuration)
       .AddControllers();

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

app.Run();
