using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace StockManagerCore.Models
{
    public class InputProduct
    {
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

        //Navigation prop
        public Product Product { get; set; }

        public InputProduct() { }

        public InputProduct(string nItem, string xProd, int qCom, double vUnCom, string uCom, double vtotal,
            double vUnTrib, double vTotTrib, Product product, DateTime dhEmi)
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

        }
    }
}
