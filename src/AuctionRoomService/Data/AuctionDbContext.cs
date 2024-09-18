using AuctionRoomService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionRoomService.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Item> Items { get; set; }
   
    }
}
