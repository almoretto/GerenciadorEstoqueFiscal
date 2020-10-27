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
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().HasData(
                 new Product { Id = 1, Group = "ANEL" },
                 new Product { Id = 2, Group = "ARGOLA" },
                 new Product { Id = 3, Group = "BRACELETE" },
                 new Product { Id = 4, Group = "BRINCO" },
                 new Product { Id = 5, Group = "CHOCER" },
                 new Product { Id = 6, Group = "COLAR" },
                 new Product { Id = 7, Group = "CORRENTE" },
                 new Product { Id = 8, Group = "PINGENTE" },
                 new Product { Id = 9, Group = "PULSEIRA" },
                 new Product { Id = 10, Group = "TORNOZELEIRA" },
                 new Product { Id = 11, Group = "PEAÇAS" },
                 new Product { Id = 12, Group = "VARIADOS" },
                 new Product { Id = 13, Group = "BROCHE" });

            modelBuilder.Entity<Company>().HasData(
                 new Company { Id = 1, Name = "ATACADAO" },
                 new Company { Id = 2, Name = "JR" });


        }
    }
}
