﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using StockManagerCore.Data;
using StockManagerCore.Services;
using System;

namespace StockManagerCore
{
    public class StartupFactory : IDesignTimeDbContextFactory<StockDBContext>
    {
        public StockDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockDBContext>();
            optionsBuilder.UseSqlServer("server=tcp:=192.168.100.2,1433;Network Library = DBMSSOCN;Initial Catalog =StockKManagerDB;User Id=sa;Password=$3nh@2018;");

            return new StockDBContext(optionsBuilder.Options);
        }
       
    }
}
