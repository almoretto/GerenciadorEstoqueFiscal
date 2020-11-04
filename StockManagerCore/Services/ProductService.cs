using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Models;

namespace StockManagerCore.Services
{
    public class ProductService
    {

        private readonly StockDBContext _context;
        public ProductService(StockDBContext context) { _context = context; }

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
    }
}
