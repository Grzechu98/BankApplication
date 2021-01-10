using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.BlazorFrontend.Models
{
    public class AddressModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string UnitNumber { get; set; }
        public string PostCode { get; set; }

    }
}
