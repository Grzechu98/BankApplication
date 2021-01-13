using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplication.SharedLibrary.Models
{
    public class AccountSettings
    {
        public int Id { get; set; }
        public int MaxDailyOperationsNumber { get; set; }
        public decimal SingleOperationLimit { get; set; }
        public decimal DailyOperationLimit { get; set; }
        public int BankAccountId { get; set; }
        [ForeignKey("BankAccountId")]
        public virtual BankAccountModel BankAccount { get; set; }

    }
}