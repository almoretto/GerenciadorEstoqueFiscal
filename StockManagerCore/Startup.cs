using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockManagerCore.Data;

namespace StockManagerCore
{
    public class StartupFactory : IDesignTimeDbContextFactory<StockDBContext>
    {
        public StockDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockDBContext>();
            optionsBuilder.UseSqlServer(@"server=192.168.100.2\SQLEXPRESS;Database=StockKManagerDB;User Id=administrator;Password=$3nh@2018;");

            return new StockDBContext(optionsBuilder.Options);
        }

    }
}
