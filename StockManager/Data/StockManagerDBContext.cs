using StockManager.Models;
using Microsoft.EntityFrameworkCore;


namespace StockManager.Data
{
    public class StockManagerDBContext : DbContext
    {
        public StockManagerDBContext(DbContextOptions<StockManagerDBContext> options)
             : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=SRV001\SQLEXPRESS;Database=StockKManagerDB;User Id=administrator;Password=$3nh@2018;");
        }

        public DbSet<InputProduct> InputProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}
