﻿#region --== Dependency declaration ==--
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Models;
#endregion

namespace StockManagerCore.Data
{
    public class StockDBContext : DbContext
    {
        public StockDBContext(DbContextOptions<StockDBContext> options) : base(options){ }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=tcp:192.168.100.2,1433;Initial Catalog =StockKManagerDB;User Id=sa;Password=$3nh@2018;");
        }

        #region --== DB Sets ==--
        public DbSet<InputProduct> InputProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<NFControl> NFControls { get; set; }
        public DbSet<Person> People { get; set; }

        #endregion


    }
}
