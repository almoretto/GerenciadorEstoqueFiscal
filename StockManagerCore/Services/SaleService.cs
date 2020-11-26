#region --== Dependency declatation ==--
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Models;
#endregion

namespace StockManagerCore.Services
{
    public class SaleService
    {
        #region --== Constructor for dependency injections ==--
        
        //Constructor and dependency injection to DBContext
        private readonly StockDBContext _context;
        public SaleService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
       
        //Method to insert multiple sales on database
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
        
        //Querry to retrieve all sales from database
        public IEnumerable<SoldProduct> GetSales()
        {
            return _context.SoldProducts
                .Include(s => s.Company)
                .Include(s => s.Product).OrderBy(s => s.Company);
        }
        
        //Querry to find sales filtering by date and company
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
        
        //Querry to find sales filtering by date.
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
        #endregion
    }
}
