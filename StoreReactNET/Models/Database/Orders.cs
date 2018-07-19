using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class Orders
    {
        public int Id { get; set; }
        public int? CartId { get; set; }

        public Carts Cart { get; set; }
    }
}
