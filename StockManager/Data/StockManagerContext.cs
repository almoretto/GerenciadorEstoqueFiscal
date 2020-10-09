using StockManager.Models;
using Microsoft.EntityFrameworkCore;


namespace StockManager.Data
{
    class StockManagerContext : DbContext
    {
        public StockManagerContext(DbContextOptions<StockManagerContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"\\srv001\SQLEXPRESS;Database=StockManagerDB;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<InputProduct> InputProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        public DbSet<Stock> Stocks { get; set; }

    }
}
