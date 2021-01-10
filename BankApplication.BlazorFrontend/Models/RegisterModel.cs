using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.BlazorFrontend.Models
{
    public class RegisterModel
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
        public string Login { get; set; }
        public string Password { get; set; }
        public AddressModel Address { get; set; }
    }
}
