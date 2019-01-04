using System;
using System.Collections.Generic;
using System.Text;

namespace StoreReactNET.Services.Account.Models.Outputs
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public List<OrderItemDTO> OrderProducts { get; set; }
        public double TotalPrice { get; set; }
    }
}
