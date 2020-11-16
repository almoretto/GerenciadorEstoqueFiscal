using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using StockManagerCore.Models.Enums;

namespace StockManagerCore.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Doc { get; set; }
        public string City { get; set; }
        [Required]
        public string  State { get; set; }
        [Required]
        public PersonType Type { get; set; }
        [Required]
        public PersonCategory Category { get; set; }


    }
}
