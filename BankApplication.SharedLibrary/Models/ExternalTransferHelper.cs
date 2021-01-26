using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.SharedLibrary.Models
{
    public class ExternalTransferHelper
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string SenderName { get; set; }
        public string RecipientName { get; set; }
        public string SenderAccountNumber { get; set; }
        public string RecipientAccountNumber { get; set; }
    }
}
