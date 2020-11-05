#region --== Dependency declaration ==--
using System.ComponentModel.DataAnnotations;
using System;
#endregion

namespace StockManagerCore.Models
{
    public class InputProduct
    {
        #region --== Model Properties ==--
        [Key]
        public int Id { get; set; }
        public string NItem { get; set; }
        public string XProd { get; set; }
        public int QCom { get; set; }
        public double VUnCom { get; set; }
        public string UCom { get; set; }
        public double Vtotal { get; set; }
        public double VUnTrib { get; set; }
        public double VTotTrib { get; set; }
        public DateTime DhEmi { get; set; }

        [Required]
        public Company Company { get; set; }

        //Navigation prop
        [Required]
        public Product Product { get; set; }
        #endregion

        #region --== Constructors ==--
        public InputProduct() { }

        public InputProduct(string nItem, string xProd, int qCom, double vUnCom, string uCom, double vtotal,
            double vUnTrib, double vTotTrib, Product product, DateTime dhEmi, Company company)
        {
            NItem = nItem;
            XProd = xProd;
            QCom = qCom;
            VUnCom = vUnCom;
            UCom = uCom;
            Vtotal = vtotal;
            VUnTrib = vUnTrib;
            VTotTrib = vTotTrib;
            Product = product;
            DhEmi = dhEmi;
            Company = company;
        }
        #endregion

    }
}

