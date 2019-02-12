using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Account.Models;
using StoreReactNET.Services.Account.Models.Inputs;
using StoreReactNET.Services.Account.Models.Outputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Services.Account
{
    public interface IAccountQueries
    {
        Task<UserDTO> GetUserByCredentialsAsync(string Email, string HashedPassword);
        Task<UserDTO> GetUserByEmailAsync(string Email);
        Task<UserDTO> GetUserByIDAsync(string userID);
        Task RegisterUserAsync(string Email, string HashedPassword);
        Task<UserDetailsDTO> GetUserDetailsAsync(string userID);
        Task<List<UserAddressDTO>> GetUserAddressesAsync(string userID);
        Task<List<OrderDTO>> GetUserOrders(string userID);
        Task<bool> SetUserDetails(int userId, UserDetailsViewModel userDetailsViewModel);
        Task<bool> SetAddress(int userId, UserAddressDTO userAddress);
        Task<bool> RemoveUserAddress(int userId, int addressId);
        Task<bool> SubmitOrder(int userId, List<CartProductDTO> cart, SentOrderViewModel sentOrder);
    }
}
