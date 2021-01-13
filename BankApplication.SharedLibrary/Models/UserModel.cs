using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.SharedLibrary.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Secondname { get; set; }
        public string Nationality { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PIN { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IdentityDocumentNumber { get; set; }
        public DateTime IdentityDocumentExpirationDate { get; set; }
        public AddressModel Address { get; set; }
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
