namespace StoreReactNET.Infrastructure.EntityFramework.Entities
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? UserDetailsId { get; set; }

        public UserDetails UserDetails { get; set; }
    }
}
