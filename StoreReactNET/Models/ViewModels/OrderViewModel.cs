using Newtonsoft.Json;
using StoreReactNET.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models.ViewModels
{
    public class OrderViewModel
    {
        [JsonProperty("orderID")]
        public int OrderID { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("orderProducts")]
        public List<OrderItemViewModel> OrderProducts { get; set; }
        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }


        public OrderViewModel(Orders order)
        {
            try
            {
                var db = new StoreASPContext();
                var orderItems = db.OrderItems
                                   .Where(c => c.OrderId == order.Id)
                                   .ToList();
                this.TotalPrice = 0;

                if(orderItems.Count > 0)
                {
                    var temp = new List<OrderItemViewModel>();
                    foreach(var item in orderItems)
                    {
                        var itemVM = new OrderItemViewModel(item);
                        temp.Add(itemVM);
                        this.TotalPrice += itemVM.ProductTotalPrice;
                    }
                    this.OrderID = order.Id;
                    this.Date = order.Date.ToShortDateString();
                    this.Status = Singleton.OrderStatuses[order.Status];
                    this.OrderProducts = temp;

                }
            }
            catch (Exception) { }
        }
    }
    public class OrderItemViewModel
    {
        [JsonProperty("productID")]
        public int ProductID { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("productTotalPrice")]
        public double ProductTotalPrice { get; set; }

        public OrderItemViewModel(OrderItems item)
        {
            try
            {
                var db = new StoreASPContext();

                var product = db.Products
                                .Where(c => c.Id == item.ProductId)
                                .FirstOrDefault();

                this.ProductID = product.Id;
                this.ProductName = product.Name;
                this.Quantity = item.Quantity;
                this.ProductTotalPrice = product.PriceVat * 1.23 * this.Quantity;
            }
            catch (Exception) { }
        }
    }
}
