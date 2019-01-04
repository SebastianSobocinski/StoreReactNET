using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreReactNET.Services.Account.Models.Outputs
{
    public class UserAddressDTO
    {
        public int? Id { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string HomeNr { get; set; }
        public string AppartmentNr { get; set; }
        [Required]
        public string Zipcode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
