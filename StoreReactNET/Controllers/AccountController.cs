using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreReactNET.Models.Database;
using StoreReactNET.Models.ViewModels;
using StoreReactNET.Services;

namespace StoreReactNET.Controllers
{
    [Route("Account/[action]")]
    public class AccountController : Controller
    {
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var db = new StoreASPContext();
            var result = db.Users
                           .Where(c => c.Email == Email && c.Password == SHA256Service.GetHashedString(Password))
                           .Include(c => c.UserDetails)
                           .FirstOrDefault();

            if (result != null)
            {
                var userVM = new UserViewModel(result);
                var userString = JsonConvert.SerializeObject(userVM);
                var cartVM = new List<CartItemViewModel>();
                var cartString = JsonConvert.SerializeObject(cartVM);

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
                var db = new StoreASPContext();

                var result = db.Users
                               .Where(c => c.Email == collection.Email)
                               .FirstOrDefault();

                if(result == null)
                {
                    var entry = new Users
                    {
                        Email = collection.Email,
                        Password = SHA256Service.GetHashedString(collection.Password)
                    };

                    await db.Users.AddAsync(entry);
                    await db.SaveChangesAsync();

                    return Redirect("/");
                }
                else
                {
                    return Redirect("/Account/Register/?failCode=0");
                }
                
            }
            else
            {
                if(collection.Password != collection.RePassword)
                {
                    return Redirect("/Account/Register/?failCode=1");
                }
                else
                {
                    return Redirect("/Account/Register/?failCode=2");
                }
            }

        }
        [HttpGet]
        public ActionResult GetUserDetails()
        {
            var respond = new
            {
                success = false,
                userDetails = new UserDetailsViewModel(null)
            };

            var session = HttpContext.Session.GetString("user");
            if(session != null)
            {
                var user = JsonConvert.DeserializeObject<UserViewModel>(session);
                if (user != null)
                {
                    var db = new StoreASPContext();
                    var result = db.Users
                                   .Include(c => c.UserDetails)
                                   .Where(c => user.ID == c.Id.ToString())
                                   .FirstOrDefault();

                    if (result != null)
                    {
                        if (result.UserDetailsId != null)
                        {
                            respond = new
                            {
                                success = true,
                                userDetails = new UserDetailsViewModel(result.UserDetails)
                            };
                        }
                    }
                }
            }
            
            return Json(respond);
        }
        [HttpGet]
        public ActionResult GetUserAddresses()
        {
            var respond = new
            {
                success = false,
                userAddresses = new List<UserAdresses>()
            };

            var session = HttpContext.Session.GetString("user");
            if(session != null)
            {
                var user = JsonConvert.DeserializeObject<UserViewModel>(session);
                if (user != null)
                {
                    var db = new StoreASPContext();
                    var result = db.UserAdresses
                                   .Where(c => user.ID == c.UserId.ToString())
                                   .ToList();

                    if (result != null)
                    {
                        respond = new
                        {
                            success = true,
                            userAddresses = result
                        };
                    }
                }
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
                    var db = new StoreASPContext();
                    var uservm = JsonConvert.DeserializeObject<UserViewModel>(session);


                    var user = await db.Users
                                       .Include(c => c.UserDetails)
                                       .Where(c => uservm.ID == c.Id.ToString())
                                       .FirstOrDefaultAsync();

                    if(user.UserDetailsId == null)
                    {
                        var entry = new UserDetails()
                        {
                            Name = collection.FirstName,
                            FullName = collection.LastName,
                            DateOfBirth = collection.DateOfBirth
                        };

                        db.UserDetails.Add(entry);
                        await db.SaveChangesAsync();
                        user.UserDetailsId = entry.Id;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        user.UserDetails.Name = collection.FirstName;
                        user.UserDetails.FullName = collection.LastName;
                        user.UserDetails.DateOfBirth = collection.DateOfBirth;

                        await db.SaveChangesAsync();
                    }
                }
            }


            return Redirect("/");
        }
        [HttpPost]
        public async Task<ActionResult> SetAddress([FromForm]UserAddressViewModel collection)
        {
            if (ModelState.IsValid)
            {
                var session = HttpContext.Session.GetString("user");
                if(session != null)
                {
                    var db = new StoreASPContext();
                    var uservm = JsonConvert.DeserializeObject<UserViewModel>(session);



                    if(collection.Id != null)
                    {
                        var result = await db.UserAdresses
                                             .Where(
                                                c => 
                                                uservm.ID == c.UserId.ToString()
                                                &&
                                                collection.Id == c.Id
                                                )
                                             .FirstOrDefaultAsync();

                        if(result != null)
                        {
                            result.StreetName = collection.StreetName;
                            result.HomeNr = collection.HomeNr;
                            result.AppartmentNr = collection.AppartmentNr;
                            result.Zipcode = collection.Zipcode;
                            result.City = collection.City;
                            result.Country = collection.Country;

                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        try
                        {
                            int userid = int.Parse(uservm.ID);
                            var entry = new UserAdresses()
                            {
                                UserId = userid,
                                StreetName = collection.StreetName,
                                HomeNr = collection.HomeNr,
                                AppartmentNr = collection.AppartmentNr,
                                Zipcode = collection.Zipcode,
                                City = collection.City,
                                Country = collection.Country
                            };
                            await db.UserAdresses.AddAsync(entry);
                            await db.SaveChangesAsync();
                        }
                        catch (Exception) { }

                    }
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
                var uservm = JsonConvert.DeserializeObject<UserViewModel>(session);

                var db = new StoreASPContext();
                var result = db.UserAdresses
                               .Where(c =>
                                    c.Id == Id
                                    &&
                                    uservm.ID == c.UserId.ToString()
                                    )
                               .FirstOrDefault();

                if(result != null)
                {
                    db.UserAdresses.Remove(result);
                    await db.SaveChangesAsync();

                    respond = new
                    {
                        success = true
                    };
                }
            }

            return Json(respond);
        }
        [HttpPost]
        public async Task<ActionResult> SubmitOrder([FromForm]OrderViewModel collection)
        {
            if (ModelState.IsValid)
            {
                var sessionUser = HttpContext.Session.GetString("user");
                var sessionCart = HttpContext.Session.GetString("cart");

                if(sessionUser != null && sessionCart != null)
                {
                    var uservm = JsonConvert.DeserializeObject<UserViewModel>(sessionUser);
                    var cartvm = JsonConvert.DeserializeObject<List<CartItemViewModel>>(sessionCart);

                    var db = new StoreASPContext();

                    var result = await db.UserAdresses
                                        .Where(c =>
                                            collection.AddressID == c.Id
                                            &&
                                            uservm.ID == c.UserId.ToString()
                                            )
                                        .FirstOrDefaultAsync();
                    if(result != null)
                    {
                        try
                        {
                            var orderEntry = new Orders()
                            {
                                UserId = int.Parse(uservm.ID),
                                UserAddressId = collection.AddressID,
                                Date = DateTime.Now,
                                Status = 0
                            };

                            await db.Orders.AddAsync(orderEntry);
                            await db.SaveChangesAsync();

                            foreach (var item in cartvm)
                            {
                                var entryItem = new OrderItems()
                                {
                                    OrderId = orderEntry.Id,
                                    ProductId = int.Parse(item.ProductID),
                                    Quantity = item.Quantity
                                };
                                await db.OrderItems.AddAsync(entryItem);
                            }
                            await db.SaveChangesAsync();
                            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(new List<CartItemViewModel>()));

                        }
                        
                        catch (Exception) { }
                        
                    }
                }
            }
            return Redirect("/");
        }
    }
}