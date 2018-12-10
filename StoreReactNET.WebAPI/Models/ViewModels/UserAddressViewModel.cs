using System.ComponentModel.DataAnnotations;

namespace StoreReactNET.WebAPI.Models.ViewModels
{
    public class UserAddressViewModel
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
