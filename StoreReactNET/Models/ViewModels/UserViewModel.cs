using Newtonsoft.Json;
using StoreReactNET.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models.ViewModels
{
    public class UserViewModel
    {
        [JsonProperty("id")]
        public string ID { get; }
        [JsonProperty("email")]
        public string Email { get; }
        [JsonProperty("firstName")]
        public string FirstName { get; }
        [JsonProperty("lastName")]
        public string LastName { get; }

        public UserViewModel(Users _user)
        {
            try
            {
                ID = _user.Id.ToString();
                Email = _user.Email;
                FirstName = _user.UserDetails.Name;
                LastName = _user.UserDetails.FullName;
            }
            catch(Exception ex) { }


        }

    }
}
