using System;
using Newtonsoft.Json;
using StoreReactNET.Infrastructure.EntityFramework.Entities;

namespace StoreReactNET.WebAPI.Models.ViewModels
{
    public class UserViewModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        public UserViewModel(Users _user)
        {
            try
            {
                ID = _user.Id.ToString();
                Email = _user.Email;
                FirstName = _user.UserDetails.Name;
                LastName = _user.UserDetails.FullName;
            }
            catch(Exception) { }


        }

    }
}
