using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Models;


namespace StockManagerCore.Services
{
    public class InputService
    {
        private readonly StockDBContext _context;
        public InputService(StockDBContext context) { _context = context; }

        public IEnumerable<InputProduct> GetInputs()
        {
            return _context.InputProducts
                .Include(i => i.Product)
                .Include(i => i.Company);

        }
        public IEnumerable<InputProduct> GetInputsByDate(DateTime date)
        {
            return _context.InputProducts
                       .Where(i => i.DhEmi.Year == date.Year
                       && i.DhEmi.Month == date.Month
                       && i.DhEmi.Day == date.Day)
                       .Include(i => i.Product)
                       .Include(i => i.Company)
                       .ToList();
        }

        public void InsertInputs(InputProduct input)
        {
            try
            {
                _context.InputProducts.Add(input);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }
    }
}
