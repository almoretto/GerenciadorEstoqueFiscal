using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Models;


namespace StockManagerCore.Services
{
    public class CompanyService
    {
        private readonly StockDBContext _context;
        public CompanyService(StockDBContext context) { _context = context; }

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
    
    
    
    
    
    }
}
