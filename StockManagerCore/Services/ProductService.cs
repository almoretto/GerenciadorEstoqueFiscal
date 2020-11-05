#region --== Dependency Declaration ==--
using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Data;
using StockManagerCore.Models;
#endregion

namespace StockManagerCore.Services
{
    public class ProductService
    {
        #region --== Constructor for dependency injection ==--
        private readonly StockDBContext _context;
        public ProductService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }
        public Product Find(Product p)
        {
            return _context.Products
                .Where(pr => pr.Id == p.Id)
                .SingleOrDefault();
        }
        public Product FindByGroup(string gr)
        {
            return _context.Products.Where(p => p.Group == gr).FirstOrDefault();
        }
        #endregion
    }
}
