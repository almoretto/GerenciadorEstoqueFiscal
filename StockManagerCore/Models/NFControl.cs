using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using StockManagerCore.Models.Enums;

namespace StockManagerCore.Models
{
    public class NFControl
    {
        #region --== Model Properties ==--
        [Key]
        public int Id { get; set; }
        [Required]
        public int NFNumber { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public int Operation { get; set; }
        [Required]
        public DateTime Expiration { get; set; }
        [Required]
        public NFType OperationType { get; set; }
        [Required]
        public Company Company { get; set; }
        [Required]
        public Person Destinatary { get; set; }


        #endregion

        #region --== Constructors ==--
        public NFControl() { }

        public NFControl(int nFNumber, double value, int operation, DateTime expiration, NFType operationType, Company company, Person destinatary)
        {
            NFNumber = nFNumber;
            Value = value;
            Operation = operation;
            Expiration = expiration;
            OperationType = operationType;
            Company = company;
            Destinatary = destinatary;
        }



        #endregion
    }
}
