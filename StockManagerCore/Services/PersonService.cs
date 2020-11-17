﻿using System.Collections.Generic;
using System.Linq;
using StockManagerCore.Data;
using StockManagerCore.Models;


namespace StockManagerCore.Services
{
    public class PersonService
    {
        #region --== Dependency Injection and constructor ==--
        private readonly StockDBContext _context;
        public PersonService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods==--
        public IEnumerable<Person> GetPeople()
        {
            return _context.People.OrderBy(c => c.Name);
        }

        #endregion
    }
}
