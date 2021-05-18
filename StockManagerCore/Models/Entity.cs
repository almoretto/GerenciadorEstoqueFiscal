namespace StockManagerCore.Models
{
	public class Entity : IEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public Entity() { }
        public Entity( int id, string description )
        {
            Id = id;
            Description = description;
        }
    }
}
