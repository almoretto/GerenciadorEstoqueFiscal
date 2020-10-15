using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagerCore.Models
{
    public class InputProduct
    {
        [Key]
        public int Id { get; set; }
        public string NItem { get; set; }
        public string XProd { get; set; }
        public int QCom { get; set; }
        public double VUnCom  { get; set; }
        public string UCom { get; set; }
        public double  Vtotal { get; set; }
        public double VUnTrib { get; set; }
        public double VTotTrib { get; set; }

       

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        //Navigation prop
        public Product ProductNav { get; set; }
    }
}
