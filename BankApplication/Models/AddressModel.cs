using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string UnitNumber { get; set; }
        [Required]
        public string PostCode { get; set; }

        public ICollection<UserModel> Residents { get; set; }
    }
}
