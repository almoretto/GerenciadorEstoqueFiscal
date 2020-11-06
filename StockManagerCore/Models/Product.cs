#region --== Dependency declaration ==--
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace StockManagerCore.Models
{
    public class Product
    {
        #region --== Model properties ==--
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string GroupP { get; set; }

        //navigation to IncomeProduct
        public ICollection<InputProduct> InputProduct { get; set; }

        //Navigation to Sales Product
        public ICollection<SoldProduct> SoldProduct { get; set; }
        #endregion

        #region --== Constructors ==--
        public Product() { }

        public Product(string group)
        {
            GroupP = group;
        }
        #endregion
    }
}
