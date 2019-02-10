using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using StoreReactNET.Infrastructure.EntityFramework;
using StoreReactNET.Infrastructure.EntityFramework.Repositories;

namespace StoreReactNET.Infrastructure.Tests
{
    [TestFixture]
    public class AccountQueriesTests
    {
        private AccountQueries _accountQueries;

        public AccountQueriesTests()
        {
            _accountQueries = new AccountQueries(
                new StoreASPContext()
                );
        }
        [Test]
        public async Task GetUserByCredentialsAsync_CorrectCredentials_ShouldReturnUser()
        {
            var correctEmail = "test@test";
            var correctPassword = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08";

            var user = await _accountQueries.GetUserByCredentialsAsync(correctEmail, correctPassword);

            Assert.That(user != null && user.Email == correctEmail);
        }
        [Test]
        public async Task GetUserByCredentials_InvalidCredentials_ShouldReturnNull()
        {
            var invalidEmail = "invalid@email";
            var invalidPassword = "invalidPas.Sword123";

            var user = await _accountQueries.GetUserByCredentialsAsync(invalidEmail, invalidPassword);

            Assert.That(user == null);
        }
        [Test]
        public async Task GetUserByEmailAsync_ExistingUser_ShouldReturnUser()
        {
            var existingEmail = "test@test";

            var user = await _accountQueries.GetUserByEmailAsync(existingEmail);

            Assert.That(user != null && user.Email == existingEmail);
        }
        [Test]
        public async Task GetUserByEmailAsync_NonExistentUser_ShouldReturnNull()
        {
            var nonExistingEmail = "invalidEmail@invalid.com";

            var user = await _accountQueries.GetUserByEmailAsync(nonExistingEmail);

            Assert.That(user == null);
        }

        [Test]
        public async Task RegisterUserAsync_UserAlreadyExists_ShouldReturnExistingUser()
        {
            var correctEmail = "test@test";
            var correctPassword = "someTestPassword";

            await _accountQueries.RegisterUserAsync(correctEmail, correctPassword);
            var user = await _accountQueries.GetUserByEmailAsync(correctEmail);

            Assert.That(user != null && user.Email == correctEmail);
        }
        [Test]
        public async Task RegisterUserAsync_UserDoesntExists_ShouldReturnNewUser()
        {
            var correctEmail = "notExistingUser@test";
            var correctPassword = "someTestPassword";

            await _accountQueries.RegisterUserAsync(correctEmail, correctPassword);
            var user = await _accountQueries.GetUserByEmailAsync(correctEmail);

            Assert.That(user != null && user.Email == correctEmail);
        }
        [Test]
        public async Task GetUserDetailsAsync_UserDoesntExists_ShouldReturnNull()
        {
            var userId = "99test99";

            var details = await _accountQueries.GetUserByEmailAsync(userId);

            Assert.That(details == null);
        }

        [Test]
        public async Task GetUserDetailsAsync_UserDoesntHaveDetails_ShouldReturnNull()
        {
            var userId = "2004";

            var details = await _accountQueries.GetUserDetailsAsync(userId);

            Assert.That(details == null);
        }

        [Test]
        public async Task GetUserDetailsAsync_UserDoesHaveDetails_ShouldReturnUser()
        {
            var userId = "2005";

            var details = await _accountQueries.GetUserDetailsAsync(userId);

            Assert.That(details != null && details.FirstName == "Sebastian");
        }
        [Test]
        public async Task GetUserAddressesAsync_UserDoesntHaveAnyAddress_ShouldReturnEmptyList()
        {
            var userId = "2003";

            var addresses = await _accountQueries.GetUserAddressesAsync(userId);

            Assert.That(addresses.Count == 0);
        }
        [Test]
        public async Task GetUserAddressesAsync_UserDoesHaveAnyAddress_ShouldReturnAddressList()
        {
            var userId = "1";

            var addresses = await _accountQueries.GetUserAddressesAsync(userId);

            Assert.That(addresses.Count > 0 && addresses[0].StreetName == "Test");
        }
    }
}
