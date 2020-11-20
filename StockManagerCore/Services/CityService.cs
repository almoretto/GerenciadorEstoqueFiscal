using StockManagerCore.Data;
using StockManagerCore.Models;
using StockManagerCore.Models.Enums;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services
{
    public class CityService
    {
        #region --== Dependency Injection and constructor ==--
        private readonly StockDBContext _context;
        public CityService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.CityName);
        }

        #endregion
    }
}
