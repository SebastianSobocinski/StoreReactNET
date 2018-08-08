using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class Users
    {
        public Users()
        {
            Carts = new HashSet<Carts>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? UserAdressId { get; set; }
        public int? UserDetailsId { get; set; }

        public UserAdresses UserAdress { get; set; }
        public UserDetails UserDetails { get; set; }
        public ICollection<Carts> Carts { get; set; }
    }
}
