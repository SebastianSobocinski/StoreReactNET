namespace StoreReactNET.Infrastructure.EntityFramework.Entities
{
    public partial class ProductImages
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public int ProductId { get; set; }

        public Products Product { get; set; }
    }
}
