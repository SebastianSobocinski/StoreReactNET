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
    }
}