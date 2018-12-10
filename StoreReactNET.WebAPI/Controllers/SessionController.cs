using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreReactNET.WebAPI.Models.ViewModels;

namespace StoreReactNET.WebAPI.Controllers
{
    [Route("Session/[action]")]
    public class SessionController : Controller
    {
        /*
        [HttpGet]
        public ActionResult GetUserSession()
        {
            var respond = new
            {
                isEstablished = false,
                user = ""
            };
            var session = HttpContext.Session.GetString("user");

            if(session != null)
            {
                respond = new
                {
                    isEstablished = true,
                    user = session
                };
            }

            return Json(respond);
        }
        [HttpGet]
        public ActionResult GetCartSession()
        {
            var respond = new
            {
                isEstablished = false,
                cart = ""
            };
            var session = HttpContext.Session.GetString("cart");

            if (session != null)
            {
                respond = new
                {
                    isEstablished = true,
                    cart = session
                };
            }

            return Json(respond);
        }
        [HttpPost]
        public ActionResult AddToCartSession(int ProductID)
        {
            var respond = new
            {
                success = false,
                message = "Some kind of error occured.",
                cart = ""
            };
            var UserSession = HttpContext.Session.GetString("user");
            if(UserSession == null)
            {
                respond = new
                {
                    success = false,
                    message = "Please log in.",
                    cart = ""
                };
            }
            else
            {
                var CartSession = HttpContext.Session.GetString("cart");
                var CartObject = JsonConvert.DeserializeObject<List<CartItemViewModel>>(CartSession);

                var db = new StoreASPContext();
                var result = db.Products
                               .Where(c => c.Id == ProductID)
                               .Include(c => c.ProductCategory)
                               .Include(c => c.ProductImages)
                               .Include(c => c.ProductDetails)
                               .FirstOrDefault();

                if(result != null)
                {
                    var ItemInCart = CheckIfInCart(ProductID, CartObject);
                    if (ItemInCart != null)
                    {
                        ItemInCart.Quantity++;
                    }
                    else
                    {
                        CartObject.Add(new CartItemViewModel(result)
                        {
                            Quantity = 1
                        });
                    }
                    
                    var CartString = JsonConvert.SerializeObject(CartObject);
                    HttpContext.Session.SetString("cart", CartString);

                    respond = new
                    {
                        success = true,
                        message = "Success",
                        cart = CartString
                    };
                }
               
            }
            

            return Json(respond);
        }
        [HttpPost]
        public ActionResult UpdateCartSession(string Cart)
        {
            var sentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(Cart);

            var respond = new
            {
                success = false,
                message = "Something went wrong.",
                cart = new List<CartItemViewModel>()
            };
            if(HttpContext.Session.GetString("user") == null)
            {
                respond = new
                {
                    success = false,
                    message = "Please log in!",
                    cart = new List<CartItemViewModel>()
                };
            }
            else
            {
                foreach (var item in sentCart.ToList())
                {
                    if (item.Quantity <= 0)
                    {
                        sentCart.Remove(item);
                    }
                }
                var sentCartString = JsonConvert.SerializeObject(sentCart);
                HttpContext.Session.SetString("cart", sentCartString);
                respond = new
                {
                    success = true,
                    message = "Updated successfully",
                    cart = sentCart
                };
            }
            


            return Json(respond);
        }
        [HttpPost]
        public ActionResult ClearSession()
        {
            var respond = new
            {
                success = false
            };

            var session = HttpContext.Session;
            session.Clear();

            respond = new
            {
                success = true
            };
            return Json(respond);
        }




        protected CartItemViewModel CheckIfInCart(int ProductID, List<CartItemViewModel> List)
        {
            foreach(var Item in List)
            {
                if(Item.ProductID == ProductID.ToString())
                {
                    return Item;
                }
            }
            return null;
        }
        */
    }
}