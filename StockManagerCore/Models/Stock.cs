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
        [Display(Name ="Produto")]
        public Product Product { get; set; }
        [Display(Name ="Qte. Comprada")]
        public int QtyPurchased { get; private set; }
        [Display(Name ="Qte. Vendida")]
        public int QtySold { get; private set; }
        [Display(Name ="Valor Compra")]
        public double AmountPurchased { get; set; }
        [Display(Name ="Valor Venda")]
        public double AmountSold { get; set; }
        [Display(Name ="Ultima Entrada")]
        public DateTime LastInput { get; set; }
        [Display(Name ="Ultima Saída")]
        public DateTime LastSales { get; set; }
        [Required]
        [Display(Name ="Empresa")]
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
        public Stock(Product product, int qtyPurchased, int qtySold, double amountPurchased,
           double amountSold, DateTime lstImput, DateTime lstSale, Company company)
        {
            Product = product;
            QtyPurchased = qtyPurchased;
            QtySold = qtySold;
            AmountPurchased = amountPurchased;
            AmountSold = amountSold;
            LastInput = lstImput;
            LastSales = lstSale;
            Company = company;
        }
        #endregion

        #region --== Methods ==--
        
        //Method to make a moviment of purchase and add itens to stock
        public void MovimentInput(int qty, double amount, DateTime lstD)
        {
            QtyPurchased += qty;
            AmountPurchased += amount;
            LastInput = lstD.Date;
        }

        //Method to make moviment of sale and subtract itens of the stock
        public void MovimentSale(int qty, double amount, DateTime lstD)
        {
            QtySold += qty;
            AmountSold += amount;
            LastSales = lstD.Date;
        }
        #endregion
    }
}
