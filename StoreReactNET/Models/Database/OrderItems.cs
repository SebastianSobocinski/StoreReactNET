using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
