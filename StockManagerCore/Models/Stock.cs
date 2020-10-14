using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockManagerCore.Models
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        public List<InputProduct>  InputProducts { get; set; }
        public List<SoldProduct> SoldProducts { get; set; }

        //Navigation
        public ICollection<InputProduct> ProductsInputs { get; set; }
        public ICollection<SoldProduct> ProductsSales { get; set; }

    }
}
