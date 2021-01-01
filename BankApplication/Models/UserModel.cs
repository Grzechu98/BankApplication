using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Secondname { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public string PlaceOfBirth { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string PIN { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string IdentityDocumentNumber { get; set; }
        [Required]
        public DateTime IdentityDocumentExpirationDate { get; set; }
        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public AddressModel Address { get; set; }
        [Required]
        public string Login { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonProperty("Password")]
        public string PasswordSetter
        {
            set { Password = value; }
        }
        public virtual ICollection<BankAccountModel> Accounts { get; set; }
    }
}
