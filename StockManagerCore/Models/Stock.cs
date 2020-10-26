using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagerCore.Models
{
    [NotMapped]
    public class Stock
    {
       
        public ICollection<InputProduct>  InputProducts { get; set; }
       
        public ICollection<SoldProduct> SoldProducts { get; set; }

   

    }
}
