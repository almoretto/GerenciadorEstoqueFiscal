namespace StockManagerCore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Group { get; set; }

        //navigation to IncomeProduct
        public InputProduct InputProduct { get; set; }

        //Navigation to Sales Product
        public SoldProduct SoldProduct { get; set; }
    }
}
