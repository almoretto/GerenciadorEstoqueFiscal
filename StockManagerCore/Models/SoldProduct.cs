using System.ComponentModel.DataAnnotations;
using System;

namespace StockManagerCore.Models
{
    public class SoldProduct
    {
        [Key]
        public int Id { get; set; }
        public string NItem { get; set; } //number
        public string XProd { get; set; } //group
        public int QCom { get; set; } //Qty
        public double VUnCom { get; set; } //Unitary Valor
        public double Vtotal { get; set; } // Total Valor
        public DateTime DhEmi { get; set; } //Input data
        public Company Company { get; set; }


        //Navigation Prop
        public Product Product { get; set; }

        public SoldProduct() { }

        public SoldProduct(string nItem, string xProd, int qCom, double vUnCom, double vtotal, DateTime dhEmi, 
            Product product, Company company)
        {
            NItem = nItem;
            XProd = xProd;
            QCom = qCom;
            VUnCom = vUnCom;
            Vtotal = vtotal;
            DhEmi = dhEmi;
            Product = product;
            Company = company;
        }
    }
}
