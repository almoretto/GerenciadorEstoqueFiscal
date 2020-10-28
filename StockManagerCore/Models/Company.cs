using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<InputProduct> InputProducts { get; set; }
        public ICollection<SoldProduct> SoldProducts { get; set; }
        public ICollection<Stock> Stocks { get; set; }

        public Company() { }

        public Company( string name)
        {
            Name = name;
        }
    }
}
