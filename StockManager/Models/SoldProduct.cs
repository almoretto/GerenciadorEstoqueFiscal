using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManager.Models
{
    public class SoldProduct
    {
        [Key]
        public int Id { get; set; }
        public  int SQty { get; set; }
        public double  SUnValue { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        //Navigation Prop
        public Product Product { get; set; }

    }
}
