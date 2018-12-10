using System.Collections.Generic;

namespace StoreReactNET.Infrastructure.EntityFramework.Entities
{
    public partial class ProductCategories
    {
        public ProductCategories()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Products> Products { get; set; }
    }
}
