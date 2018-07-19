using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class Carts
    {
        public Carts()
        {
            CartItems = new HashSet<CartItems>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }

        public Users User { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
