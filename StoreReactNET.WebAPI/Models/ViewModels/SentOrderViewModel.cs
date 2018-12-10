using System.ComponentModel.DataAnnotations;

namespace StoreReactNET.WebAPI.Models.ViewModels
{
    
    public class SentOrderViewModel
    {
        [Required]
        public int AddressID { get; set; }
    }
}
