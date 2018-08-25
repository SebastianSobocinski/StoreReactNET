using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models.ViewModels
{
    
    public class OrderViewModel
    {
        [Required]
        public int AddressID { get; set; }
    }
}
