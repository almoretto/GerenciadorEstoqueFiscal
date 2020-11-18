using StockManagerCore.Models.Enums;

namespace StockManagerCore.Models
{
    public class City
    {

        public int Id { get; set; }
        public string CityName { get; set; }
        public State State { get; set; }

        public City() { }
        public City(string cityName, State state)
        {
            CityName = cityName;
            State = state;
        }
    }
}
