#region --== Dependency declaration ==--
using System.ComponentModel.DataAnnotations;
using System;
#endregion

namespace StockManagerCore.Models
{
    public class SoldProduct
    {
        #region --== Model properties ==--
        [Key]
        public int Id { get; set; }
        public string NItem { get; set; } //number
        public string XProd { get; set; } //group
        public int QCom { get; set; } //Qty
        public double VUnCom { get; set; } //Unitary Valor
        public double Vtotal { get; set; } // Total Valor
        public DateTime DhEmi { get; set; } //Input data
        [Required]
        public Company Company { get; set; }
        //Navigation Prop
        [Required]
        public Product Product { get; set; }
        #endregion

        #region --== Constructors ==--
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
        #endregion
    }
}
