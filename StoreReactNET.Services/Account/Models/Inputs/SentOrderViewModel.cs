using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreReactNET.Services.Account.Models.Inputs
{
    public class SentOrderViewModel
    {
        [Required]
        public int AddressID { get; set; }
    }
}
