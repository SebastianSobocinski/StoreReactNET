using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Services.Session
{
    public interface ISessionService
    {
        Task<ProductDTO> GetCartProduct(int ProductID);
    }
}
