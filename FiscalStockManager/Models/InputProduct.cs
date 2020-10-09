using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiscalStockManager.Models
{
    class InputProduct
    {
        [Key]
        public int Id { get; set; }
        public int IQty { get; set; }
        public double UnValue { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        
        //Navigation prop
        public Product ProductNav { get; set; }
    }
}
