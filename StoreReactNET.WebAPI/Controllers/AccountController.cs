using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreReactNET.Services.Account;
using StoreReactNET.Services.Account.Models.Inputs;
using StoreReactNET.Services.Account.Models.Outputs;
using StoreReactNET.Services.Product.Models.Outputs;


namespace StoreReactNET.WebAPI.Controllers
{
    [Route("Account/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }
        [HttpPost]
        public async Task<ActionResult> Login(string Email, string Password)
        {
            var result = await _accountService.Login(Email, Password);
            if (result != null)
            {
                var userString = JsonConvert.SerializeObject(result);
                var cartString = JsonConvert.SerializeObject(new List<CartProductDTO>());

                HttpContext.Session.SetString("user", userString);
                HttpContext.Session.SetString("cart", cartString);

                return Redirect("/");
            }
            else
            {
                return Redirect("/Account/Login/?failed=true");
            }
           
        }
        [HttpPost]
        public async Task<ActionResult> Register([FromForm]RegisterViewModel collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _accountService.Register(collection.Email, collection.Password);
                    return Redirect("/");
                }
                catch (Exception)
                {
                    return Redirect("/Account/Register/?failCode=0");
                }
            }
            else
            {
                return Redirect(collection.Password != collection.RePassword ? 
                    "/Account/Register/?failCode=1" : "/Account/Register/?failCode=2");
            }

        }
        [HttpGet]
        public async Task<ActionResult> GetUserDetails()
        {
            var respond = new
            {
                success = false,
                userDetails = new UserDetailsDTO()
            };

            var session = HttpContext.Session.GetString("user");
            if(session != null)
            {
                try
                {
                    var details = await _accountService.GetUserDetails(
                        JsonConvert.DeserializeObject<UserDTO>(session).ID
                    );

                    respond = new
                    {
                        success = true,
                        userDetails = details
                    };
                }
                catch (Exception) { }

            }
            
            return Json(respond);
        }
        [HttpGet]
        public async Task<ActionResult> GetUserAddresses()
        {
            var respond = new
            {
                success = false,
                userAddresses = new List<UserAddressDTO>()
            };

            var session = HttpContext.Session.GetString("user");
            if(session != null)
            {
                try
                {
                    var addressess = await _accountService.GetUserAddresses(
                        JsonConvert.DeserializeObject<UserDTO>(session).ID
                    );
                    respond = new
                    {
                        success = true,
                        userAddresses = addressess
                    };
                }
                catch(Exception) { }
            }
            
            return Json(respond);
        } 
        [HttpGet]
        public async Task<ActionResult> GetUserLatestOrders()
        {
            var respond = new
            {
                success = false,
                userOrders = new List<OrderDTO>()
            };

            var session = HttpContext.Session.GetString("user");
            if(session != null)
            {
                try
                {
                    var orders = await _accountService.GetUserLatestOrders(
                        JsonConvert.DeserializeObject<UserDTO>(session).ID
                        );
                    respond = new
                    {
                        success = true,
                        userOrders = orders
                    };
                }
                catch (Exception) { }
            }


            return Json(respond);
        }

        [HttpPost]
        public async Task<ActionResult> SetDetails([FromForm]UserDetailsViewModel collection)
        {
            if (ModelState.IsValid)
            {
                var session = HttpContext.Session.GetString("user");
                if(session != null)
                {
                    try
                    {
                        var uservm = JsonConvert.DeserializeObject<UserDTO>(session);
                        await _accountService.SetUserDetails(int.Parse(uservm.ID), collection);
                    }
                    catch(Exception) { }
                }
            }


            return Redirect("/");
        }
        [HttpPost]
        public async Task<ActionResult> SetAddress([FromForm]UserAddressDTO collection)
        {
            if (ModelState.IsValid)
            {
                var session = HttpContext.Session.GetString("user");
                if(session != null)
                {
                    try
                    {
                        var uservm = JsonConvert.DeserializeObject<UserDTO>(session);
                        await _accountService.SetAddress(int.Parse(uservm.ID), collection);


                    }
                    catch(Exception) { }
                }
            }


            return Redirect("/");
        }
        [HttpPost]
        public async Task<ActionResult> RemoveUserAddress(int Id)
        {
            var respond = new
            {
                success = false
            };

            var session = HttpContext.Session.GetString("user");
            if(session != null)
            {
                try
                {
                    var uservm = JsonConvert.DeserializeObject<UserDTO>(session);
                    await _accountService.RemoveUserAddress(int.Parse(uservm.ID), Id);
                    respond = new
                    {
                        success = true
                    };
                }
                catch (Exception) { }
            }

            return Json(respond);
        }
        [HttpPost]
        public async Task<ActionResult> SubmitOrder([FromForm]SentOrderViewModel collection)
        {
            if (ModelState.IsValid)
            {
                var sessionUser = HttpContext.Session.GetString("user");
                var sessionCart = HttpContext.Session.GetString("cart");

                if(sessionUser != null && sessionCart != null)
                {
                    try
                    {
                        var uservm = JsonConvert.DeserializeObject<UserDTO>(sessionUser);
                        var cartvm = JsonConvert.DeserializeObject<List<CartProductDTO>>(sessionCart);

                        await _accountService.SubmitOrder(int.Parse(uservm.ID), cartvm, collection);

                        //if succeded clear cart
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(new List<CartProductDTO>()));
                    }
                    catch (Exception) { }
                }

            }
            return Redirect("/");
        }
    }
    
}