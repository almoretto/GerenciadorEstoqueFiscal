using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StockManagerCore.Models
{
    public class Company
    {
        [Required]
        public int Id { get; set; }
        [Required]
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
