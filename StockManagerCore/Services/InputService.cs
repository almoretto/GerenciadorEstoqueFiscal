#region --== Dependency declaration ==--
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Models;
#endregion

namespace StockManagerCore.Services
{
    public class InputService
    {
        #region --== Constructor for dependency injector ==--
        private readonly StockDBContext _context;
        public InputService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
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
        public IEnumerable<InputProduct> GetInputsByDateAndCompany(DateTime date, Company co)
        {
            return _context.InputProducts
                       .Where(i => i.Company.Id == co.Id)
                       .Where(i => i.DhEmi.Year == date.Year
                       && i.DhEmi.Month == date.Month
                       && i.DhEmi.Day == date.Day)
                       .Include(i => i.Product)
                       .Include(i => i.Company);
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
        #endregion
    }
}
