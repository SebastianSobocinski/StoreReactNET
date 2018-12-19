using System.ComponentModel.DataAnnotations;

namespace StoreReactNET.Services.Account.Models.Inputs
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-mail address is required.")]
        [EmailAddress(ErrorMessage = "Invalid e-mail address." )]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [StringLength(25, MinimumLength = 6)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords must be the same.")]
        [StringLength(25, MinimumLength = 6)]
        public string RePassword { get; set; }
    }
}
