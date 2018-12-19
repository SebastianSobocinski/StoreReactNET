
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreReactNET.Services.Account;
using StoreReactNET.Infrastructure.EntityFramework.Entities;
using StoreReactNET.Services.Account.Models;
using StoreReactNET.Services.Account.Models.Outputs;

namespace StoreReactNET.Infrastructure.EntityFramework.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly StoreASPContext _context;
        public AccountRepository(StoreASPContext context)
        {
            this._context = context;
        }

        public async Task<UserDTO> GetUserByCredentialsAsync(string Email, string HashedPassword)
        {
            var result = await _context.Users
                    .Where(c => c.Email == Email && c.Password == HashedPassword)
                    .Include(c => c.UserDetails)
                    .FirstOrDefaultAsync();

            if (result != null)
            {
                return new UserDTO()
                {
                    ID = result.Id.ToString(),
                    Email = result.Email,
                    FirstName = result.UserDetails.Name,
                    LastName = result.UserDetails.FullName
                };
            }

            return null;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string Email)
        {
            var result = await _context.Users
                    .Where(c => c.Email == Email)
                    .Include(c => c.UserDetails)
                    .FirstOrDefaultAsync();

            if (result != null)
            {
                return new UserDTO()
                {
                    ID = result.Id.ToString(),
                    Email = result.Email,
                    FirstName = result.UserDetails.Name,
                    LastName = result.UserDetails.FullName
                };
            }

            return null;
        }

        public async Task<UserDTO> GetUserByIDAsync(string userID)
        {
            var result = await _context.Users
                    .Include(c => c.UserDetails)
                    .Where(c => c.Id.ToString() == userID)
                    .FirstOrDefaultAsync();

            if (result != null)
            {
                return new UserDTO()
                {
                    ID = result.Id.ToString(),
                    Email = result.Email,
                    FirstName = result.UserDetails.Name,
                    LastName = result.UserDetails.FullName
                };
            }

            return null;
        }

        public async Task RegisterUserAsync(string Email, string HashedPassword)
        {
            var entry = new Users
            {
                Email = Email,
                Password = HashedPassword
            };

            await _context.Users.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDetailsDTO> GetUserDetailsAsync(string userID)
        {
            var user = await _context.Users
                    .Include(c => c.UserDetails)
                    .Where(c => userID == c.Id.ToString())
                    .FirstOrDefaultAsync();

            if (user.UserDetails != null)
            {
                return new UserDetailsDTO()
                {
                    FirstName = user.UserDetails.Name,
                    DateOfBirth = user.UserDetails.DateOfBirth,
                    LastName = user.UserDetails.FullName
                };
            }

            return null;
        }
    }
}
