using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Models;

namespace StockManagerCore.Services
{
    public class SaleService
    {
        private readonly StockDBContext _context;
        public SaleService(StockDBContext context) { _context = context; }
        public void InsertMultiSales(List<SoldProduct> sales)
        {
            try
            {
                _context.SoldProducts.AddRange(sales);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }
        public IEnumerable<SoldProduct> GetSales()
        {
            return _context.SoldProducts
                .Include(s => s.Company)
                .Include(s => s.Product).OrderBy(s => s.Company);
        }
        public IEnumerable<SoldProduct> GetSalesByDateAndCompany(DateTime di, DateTime df, Company co)
        {
            return _context.SoldProducts
                .Include(s => s.Product)
                .Include(s => s.Company)
                .Where(c => c.Company.Id == co.Id)
                .Where(s => s.DhEmi.Year >= di.Year
                && s.DhEmi.Month >= di.Month
                && s.DhEmi.Day >= di.Day
                && s.DhEmi.Year <= df.Year
                && s.DhEmi.Month <= df.Month
                && s.DhEmi.Day <= df.Day);
        }
        public IEnumerable<SoldProduct> GetSalesByDate(DateTime di, DateTime df)
        {
            return _context.SoldProducts
                .Include(s => s.Product)
                .Include(s => s.Company)
                .Where(s => s.DhEmi.Year >= di.Year
                && s.DhEmi.Month >= di.Month
                && s.DhEmi.Day >= di.Day
                && s.DhEmi.Year <= df.Year
                && s.DhEmi.Month <= df.Month
                && s.DhEmi.Day <= df.Day);
        }
    }
}
