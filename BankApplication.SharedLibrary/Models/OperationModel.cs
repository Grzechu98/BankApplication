using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.SharedLibrary.Models
{
    public class OperationModel : IOperation
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Value { get; set; }
        public string RecipientAccountNumber { get; set; }
        public int? RecipientId { get; set; }
        public int SenderId { get; set; }
        [NotMapped]
        public bool Incoming { get; set; }
        [ForeignKey("RecipientId")]
        public virtual BankAccountModel Recipient { get; set; }
        [ForeignKey("SenderId")]
        public virtual BankAccountModel Sender { get; set; }


    }
}
