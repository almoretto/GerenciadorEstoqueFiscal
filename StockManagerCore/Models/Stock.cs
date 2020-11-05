#region --== Dependencies declaration ==--
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace StockManagerCore.Models
{
    public class Stock
    {
        #region --== Model Properties ==--
        [Key]
        public int Id { get; set; }
        [Required]
        public Product Product { get; set; }
        public int QtyPurchased { get; private set; }
        public int QtySold { get; private set; }
        public double AmountPurchased { get; set; }
        public double AmountSold { get; set; }
        public DateTime LastInput { get; set; }
        public DateTime LastSales { get; set; }
        [Required]
        public Company Company { get; set; }
        #endregion

        #region --== Class Properties Not Mapped ==--
        [NotMapped]
        public List<InputProduct> InputProducts { get; set; }
        [NotMapped]
        public List<SoldProduct> SoldProducts { get; set; }
        #endregion

        #region --== Constructors ==--
        public Stock() { }
        public Stock(Product product, int qtyPurchased, int qtySold, double amountPurchased,
            double amountSold, DateTime lstImput, Company company)
        {
            Product = product;
            QtyPurchased = qtyPurchased;
            QtySold = qtySold;
            AmountPurchased = amountPurchased;
            AmountSold = amountSold;
            LastInput = lstImput;
            Company = company;
        }
        #endregion

        #region --== Methods ==--
        public void MovimentInput(int qty, double amount, DateTime lstD)
        {
            QtyPurchased += qty;
            AmountPurchased += amount;
            LastInput = lstD.Date;
        }
        public void MovimentSale(int qty, double amount, DateTime lstD)
        {
            QtySold += qty;
            AmountSold += amount;
            LastSales = lstD.Date;
        }
        #endregion
    }
}
