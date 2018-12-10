using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using StoreReactNET.Infrastructure.EntityFramework.Entities;

namespace StoreReactNET.WebAPI.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        [JsonProperty("firstName")]
        [Required]
        [StringLength(35, MinimumLength = 3)]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        [Required]
        [StringLength(35, MinimumLength = 3)]
        public string LastName { get; set; }
        [JsonProperty("dateOfBirth")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; set; }

        public UserDetailsViewModel(UserDetails details)
        {
            try
            {
                this.FirstName = details.Name;
                this.LastName = details.FullName;
                this.DateOfBirth = details.DateOfBirth;
            }
            catch (Exception) { }
        }
        public UserDetailsViewModel() { }
    }
}
