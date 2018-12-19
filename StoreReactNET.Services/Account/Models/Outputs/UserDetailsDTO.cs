using System;
using System.ComponentModel.DataAnnotations;

namespace StoreReactNET.Services.Account.Models.Outputs
{

    public class UserDetailsDTO
    {
        [Required]
        [StringLength(35, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(35, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; set; }
    }
}
