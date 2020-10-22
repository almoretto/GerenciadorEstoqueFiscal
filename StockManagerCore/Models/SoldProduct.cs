using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace StockManagerCore.Models
{
    public class SoldProduct
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

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        //Navigation Prop
        public Product Product { get; set; }

        public SoldProduct(string nItem, string xProd, int qCom, double vUnCom, string uCom, double vtotal, 
            double vUnTrib, double vTotTrib, int productId, DateTime dhEmi)
        {
            
            NItem = nItem;
            XProd = xProd;
            QCom = qCom;
            VUnCom = vUnCom;
            UCom = uCom;
            Vtotal = vtotal;
            VUnTrib = vUnTrib;
            VTotTrib = vTotTrib;
            ProductId = productId;
            DhEmi = dhEmi;
        }
    }
}
