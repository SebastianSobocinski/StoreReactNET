using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class CartItems
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string Quantity { get; set; }

        public Carts Cart { get; set; }
        public Products Product { get; set; }
    }
}
