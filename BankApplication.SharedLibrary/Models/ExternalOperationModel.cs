using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankApplication.SharedLibrary.Models
{
    public class ExternalOperationModel : IOperation
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Value { get; set; }
        public bool Incoming { get; set; }
        public string FullName { get; set; }
        public string ExternalAccountNumber { get; set; }
        public int TargetInternalAccountId { get; set; }
        [ForeignKey("TargetInternalAccountId")]
        public virtual BankAccountModel TargetInternalAccount { get; set; }
    }
}
