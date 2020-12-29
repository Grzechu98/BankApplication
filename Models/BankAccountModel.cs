using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.Models
{
    public class BankAccountModel
    {
        public int Id { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public int SettingsId { get; set; }
        [Required]
        public virtual UserModel User { get; set; }
        public virtual ICollection<OperationModel> Incomings { get; set; }
        public virtual ICollection<OperationModel> Outgoings { get; set; }
        public virtual AccountSettings Settings { get; set; }

    }
}
