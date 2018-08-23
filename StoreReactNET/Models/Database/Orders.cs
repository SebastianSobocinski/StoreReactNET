using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class Orders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserAddressId { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
    }
}
