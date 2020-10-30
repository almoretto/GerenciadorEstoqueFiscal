using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagerCore.Models
{

    public class Stock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Product Product { get; set; }
        public int QtyPurchased { get; set; }
        public int QtySold { get; set; }
        public double AmountPurchased { get; set; }
        public double AmountSold { get; set; }
        public DateTime CalcDate { get; set; }
        [Required]
        public Company Company { get; set; }

        [NotMapped]
        public List<InputProduct> InputProducts { get; set; }
        [NotMapped]
        public List<SoldProduct> SoldProducts { get; set; }

        public Stock() { }

        public Stock(Product product, int qtyPurchased, int qtySold, double amountPurchased,
            double amountSold, DateTime calcDate, Company company)
        {

            Product = product;
            QtyPurchased = qtyPurchased;
            QtySold = qtySold;
            AmountPurchased = amountPurchased;
            AmountSold = amountSold;
            CalcDate = calcDate;
            Company = company;

        }

        public void MovimentInput(Product p, int qty, double amount, DateTime d, Company c)
        {
           Product = p;
           QtyPurchased += qty;
           AmountPurchased += amount;
           CalcDate = d.Date;
           Company = c;
        }
        public void MovimentSale(Product p, int qty, double amount, DateTime d, Company c)
        {
            Product = p;
            QtySold += qty;
            AmountSold += amount;
            CalcDate = d.Date;
            Company = c;
        }
    }
}
