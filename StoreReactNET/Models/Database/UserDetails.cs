using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class UserDetails
    {
        public UserDetails()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
