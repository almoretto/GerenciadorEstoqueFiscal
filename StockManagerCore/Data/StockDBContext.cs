#region --== Dependency declaration ==--
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Models;
#endregion

namespace StockManagerCore.Data
{
    public class StockDBContext : DbContext
    {
        //DbContext dependency injection
        public StockDBContext(DbContextOptions<StockDBContext> options) : base(options){ }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=tcp:192.168.100.2,1433;Initial Catalog =StockKManagerDB;User Id=sa;Password=$3nh@2020DBAdm1m;");
        }

        #region --== DB Sets table links ==--

        //Tables conections declaration dor codeFirst approuch on EFcore
        public DbSet<InputProduct> InputProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }



        #endregion


    }
}
