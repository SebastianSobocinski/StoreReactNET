namespace StoreReactNET.Infrastructure.EntityFramework.Entities
{
    public partial class UserAdresses
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string StreetName { get; set; }
        public string HomeNr { get; set; }
        public string AppartmentNr { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
