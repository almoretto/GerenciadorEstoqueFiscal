#region --== Dependency declaration ==--
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Models;
#endregion

namespace StockManagerCore.Services
{
    public class StockService
    {
        #region --== Constructor for dependency injection ==--
        private readonly StockDBContext _context;
        public StockService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        public IEnumerable<Stock> GetStocksByCompany(Company company)
        {
            return _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Company)
                .Where(s => s.Company.Id == company.Id);
        }
        public Stock GetStockByCompanyAndGroup(Company co, string grp)
        {
            return _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Company)
                .Where(s => s.Company.Id == co.Id)
                .Where(s=>s.Product.Group==grp)
                .FirstOrDefault();
        }
        public IEnumerable<Stock> GetStocks()
        {
            return _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Company)
                .OrderBy(s => s.Product.Group);
        }
        public void UpdateStock(Stock stk)
        {
            try
            {
                _context.Stocks.Update(stk);
                _context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }
        #endregion
    }
}