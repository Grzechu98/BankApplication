using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.SharedLibrary.Models
{
    public interface IOperation
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Value { get; set; }
        public bool Incoming { get; set; }
    }
}
