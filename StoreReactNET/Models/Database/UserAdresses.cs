using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class UserAdresses
    {
        public UserAdresses()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string StreetName { get; set; }
        public string HomeNr { get; set; }
        public string AppartmentNr { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
