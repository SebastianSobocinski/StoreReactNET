using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Account.Models;
using StoreReactNET.Services.Account.Models.Inputs;
using StoreReactNET.Services.Account.Models.Outputs;

namespace StoreReactNET.Services.Account
{
    public interface IAccountService
    {
        Task<UserDTO> Login(string Email, string Password);
        Task Register(string Email, string Password);
        Task<UserDetailsDTO> GetUserDetails(string userID);
        Task<List<UserAddressDTO>> GetUserAddresses(string userID);
        Task<List<OrderDTO>> GetUserLatestOrders(string userID);
        Task SetUserDetails(int userId, UserDetailsViewModel userDetailsViewModel);
        Task SetAddress(int userId, UserAddressDTO userAddress);
        Task RemoveUserAddress(int userId, int addressId);
    }
}
