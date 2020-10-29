using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockManagerCore.Models
{
    public class Product
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Group { get; set; }

        //navigation to IncomeProduct
        public ICollection<InputProduct> InputProduct { get; set; }

        //Navigation to Sales Product
        public ICollection<SoldProduct> SoldProduct { get; set; }

        public Product() { }

        public Product(string group)
        {

            Group = group;
        }
    }
}
