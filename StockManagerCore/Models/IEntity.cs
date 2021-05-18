namespace StockManagerCore.Models
{
	public interface IEntity
    {
        int Id { get; set; }
        string Description { get; set; }
    }
}
