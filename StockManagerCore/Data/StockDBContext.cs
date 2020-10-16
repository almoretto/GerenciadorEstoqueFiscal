using Microsoft.EntityFrameworkCore;
using StockManagerCore.Models;

namespace StockManagerCore.Data
{
    public class StockDBContext : DbContext
    {
        public StockDBContext(DbContextOptions<StockDBContext> options)
           : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=tcp:192.168.100.2,1433;Initial Catalog =StockKManagerDB;User Id=sa;Password=$3nh@2018;");
        }

        public DbSet<InputProduct> InputProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                 new Product { Id = 1, Group = "Anel" },
                 new Product { Id = 2, Group = "Argola" },
                 new Product { Id = 3, Group = "Bracelete" },
                 new Product { Id = 4, Group = "Brinco" },
                 new Product { Id = 5, Group = "Choker" },
                 new Product { Id = 6, Group = "Colar" },
                 new Product { Id = 7, Group = "Corrente" },
                 new Product { Id = 8, Group = "Pingente" },
                 new Product { Id = 9, Group = "Pulseira" },
                 new Product { Id = 10, Group = "Tornozeleira" },
                 new Product { Id = 11, Group = "Peças Montagem" },
                 new Product { Id = 12, Group = "Variados" },
                 new Product { Id = 13, Group = "Broche" });
        }
    }
}
