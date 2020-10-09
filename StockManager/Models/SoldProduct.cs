using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManager.Models
{
    class SoldProduct
    {
        [Key]
        public int Id { get; set; }
        public  int SQty { get; set; }
        public double  SUnValue { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        //Navigation Prop
        public Product ProductNav { get; set; }

    }
}
