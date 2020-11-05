#region --== Dependency declaration ==--
using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Data;
using StockManagerCore.Models;
#endregion


namespace StockManagerCore.Services
{
    public class CompanyService
    {
        #region --== Dependency Injection and constructor ==--
        private readonly StockDBContext _context;
        public CompanyService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        public IEnumerable<Company> GetCompanies()
        {
            return _context.Companies.OrderBy(c => c.Name); 
        }
        public Company Find(Company c)
        {
            return _context.Companies
                .Where(co => co.Id == c.Id)
                .SingleOrDefault();
        }
        public Company FindByName(string name)
        {
            return _context.Companies.Where(c => c.Name == name).SingleOrDefault();
        }
        #endregion
    }
}
