using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Account.Models;
using StoreReactNET.Services.Account.Models.Outputs;

namespace StoreReactNET.Services.Account
{
    public interface IAccountRepository
    {
        Task<UserDTO> GetUserByCredentialsAsync(string Email, string HashedPassword);
        Task<UserDTO> GetUserByEmailAsync(string Email);
        Task<UserDTO> GetUserByIDAsync(string userID);
        Task RegisterUserAsync(string Email, string HashedPassword);
        Task<UserDetailsDTO> GetUserDetailsAsync(string userID);
        Task<List<UserAddressDTO>> GetUserAddressesAsync(string userID);
        Task<List<OrderDTO>> GetUserOrders(string userID);
    }
}
