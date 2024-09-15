using InvoiceService.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceService.Data
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Invoice> Invoices { get; set; }
    }
}
