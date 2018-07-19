using StoreReactNET.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models.ViewModels
{
    public class UserViewModel
    {
        public string ID { get; }
        public string Email { get; }
        public string FirstName { get; }
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
