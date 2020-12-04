#region --== Dependency declaration ==--
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace StockManagerCore.Models
{
    public class Company
    {
        #region --== Model properties ==--
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double MaxRevenues { get; set; }
        public double Balance { get; private set; }
        public ICollection<InputProduct> InputProducts { get; set; }
        public ICollection<SoldProduct> SoldProducts { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<NFControl> NFControls { get; set; }
        #endregion

        #region --== Constructors ==--
        public Company() { }
        public Company( string name, double maxRev)
        {
            Name = name;
            MaxRevenues = maxRev;
        }
        #endregion

        #region --== Methods ==--
        public void SetBalance(double billed)
        {
            Balance = MaxRevenues - billed;
        }

        #endregion
    }
}
