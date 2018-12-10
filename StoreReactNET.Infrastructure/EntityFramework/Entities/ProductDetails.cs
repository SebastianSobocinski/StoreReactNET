using System.Collections.Generic;

namespace StoreReactNET.Infrastructure.EntityFramework.Entities
{
    public partial class ProductDetails
    {
        public ProductDetails()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Brand { get; set; }
        public string Line { get; set; }
        public string Model { get; set; }
        public int? Vram { get; set; }
        public int? BusBandwith { get; set; }
        public int? CoreBaseClockMhz { get; set; }
        public int? CoreBoostClockMhz { get; set; }
        public int? PhysicalCores { get; set; }
        public int? Threads { get; set; }
        public string Litography { get; set; }

        public Products Product { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}
