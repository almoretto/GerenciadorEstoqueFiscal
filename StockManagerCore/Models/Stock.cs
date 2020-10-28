using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagerCore.Models
{

    public class Stock
    {

        public int Id { get; set; }
        public Product Product { get; set; }
        public int QtyPurchased { get; set; }
        public int QtySold { get; set; }
        public double AmountPurchased { get; set; }
        public double AmountSold { get; set; }
        public DateTime CalcDate { get; set; }
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


    }
}
