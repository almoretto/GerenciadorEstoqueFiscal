using System.Collections.Generic;

namespace StockManagerCore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Group { get; set; }

        //navigation to IncomeProduct
        public ICollection<InputProduct> InputProduct { get; set; }

        //Navigation to Sales Product
        public ICollection<SoldProduct> SoldProduct { get; set; }
        
        public Product() { }

        public Product( string group)
        {

            Group = group;
        }
    }
}
