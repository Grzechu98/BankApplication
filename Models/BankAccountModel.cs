using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required]
        public int UserId { get; set; }
        [Required]
        public int SettingsId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
        public virtual ICollection<OperationModel> Incomings { get; set; }
        public virtual ICollection<OperationModel> Outgoings { get; set; }
        [ForeignKey("SettingsId")]
        public virtual AccountSettings Settings { get; set; }

    }
}
