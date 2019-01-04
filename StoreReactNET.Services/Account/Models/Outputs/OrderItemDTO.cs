using System;
using System.Collections.Generic;
using System.Text;

namespace StoreReactNET.Services.Account.Models.Outputs
{
    public class OrderItemDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double ProductTotalPrice { get; set; }
    }
}
