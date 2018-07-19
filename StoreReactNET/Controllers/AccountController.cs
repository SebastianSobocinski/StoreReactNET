using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreReactNET.Models.Database;
using StoreReactNET.Models.ViewModels;


namespace StoreReactNET.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var db = new StoreASPContext();
            var respond = new
            {
                success = false
            };
            var result = db.Users
                           .Where(c => c.Email == Email && c.Password == Password)
                           .Include(c => c.UserDetails)
                           .FirstOrDefault();

            if (result != null)
            {
                var userVM = new UserViewModel(result);
                var userString = JsonConvert.SerializeObject(userVM);
                HttpContext.Session.SetString("user", userString);
                respond = new
                {
                    success = true
                };

            }
            return Json(respond);
        }
    }
}