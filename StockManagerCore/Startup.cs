#region --== Dependency declaration ==--
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StockManagerCore.Data;
#endregion

namespace StockManagerCore
{
    public class StartupFactory : IDesignTimeDbContextFactory<StockDBContext>
    {
        public StockDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockDBContext>();
            optionsBuilder.UseSqlServer("server=tcp:=192.168.100.2,1433;Network Library = DBMSSOCN;Initial Catalog =StockKManagerDB_TST;User Id=appuser;Password=$3nh@2021;");

            return new StockDBContext(optionsBuilder.Options);
        }
       
    }
}
