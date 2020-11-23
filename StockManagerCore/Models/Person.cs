using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockManagerCore.Models.Enums;

namespace StockManagerCore.Models
{
    public class Person
    {
        #region --== Class Properties ==--
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Doc { get; set; }
        public City City { get; set; }
        [Required]
        public State State { get; set; }
        [Required]
        public PersonType Type { get; set; }
        [Required]
        public PersonCategory Category { get; set; }

        //Nav
        public ICollection<NFControl> NFs { get; set; }
        #endregion

        #region --== Constructors ==--
        public Person() { }
        public Person(string name, string doc, City city, State state, PersonType type, PersonCategory category)
        {
            Name = name;
            Doc = doc;
            City = city;
            State = state;
            Type = type;
            Category = category;
        }
        #endregion

        #region --== Methods ==--

        #endregion
    }
}
