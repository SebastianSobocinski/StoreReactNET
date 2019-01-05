using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreReactNET.Services.Product.Models.Outputs;
using StoreReactNET.Services.Session;

namespace StoreReactNET.WebAPI.Controllers
{
    [Route("Session/[action]")]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
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
                cart = new List<CartProductDTO>()
            };
            var session = HttpContext.Session.GetString("cart");

            if (session != null)
            {
                respond = new
                {
                    isEstablished = true,
                    cart = JsonConvert.DeserializeObject<List<CartProductDTO>>(session)
                };
            }

            return Json(respond);
        }
        [HttpPost]
        public async Task<ActionResult> AddToCartSession(int ProductID)
        {
            var respond = new
            {
                success = false,
                message = "Some kind of error occured.",
                cart = new List<CartProductDTO>()
            };
            //checks if logged in
            var UserSession = HttpContext.Session.GetString("user");
            if(UserSession == null)
            {
                respond = new
                {
                    success = false,
                    message = "Please log in.",
                    cart = new List<CartProductDTO>()
                };
            }
            else
            {
                //gets cart
                var CartObject = JsonConvert.DeserializeObject<List<CartProductDTO>>(
                    HttpContext.Session.GetString("cart")
                    );

                try
                {
                    //tries to add to cart
                    var item = await _sessionService.GetCartProduct(ProductID);
                    var itemInCart = CheckIfInCart(ProductID, CartObject);

                    //checks if already exists in cart if so adds quantity
                    if (itemInCart != null)
                        itemInCart.Quantity++;
                    else
                    {
                        //else adds product to cart
                        CartObject.Add(new CartProductDTO()
                        {
                            ProductCategoryID = item.ProductCategoryID,
                            ProductCategoryName = item.ProductCategoryName,
                            ProductDescription = item.ProductDescription,
                            ProductID = item.ProductID,
                            ProductImages = item.ProductImages,
                            ProductName = item.ProductName,
                            ProductPrice = item.ProductPrice,
                            Quantity = 1
                        });
                    }

                    //deserialize to string
                    var CartString = JsonConvert.SerializeObject(CartObject);
                    HttpContext.Session.SetString("cart", CartString);

                    respond = new
                    {
                        success = true,
                        message = "Success",
                        cart = CartObject
                    };

                }
                catch(Exception) { }
               
            }
            return Json(respond);
        }
        [HttpPost]
        public ActionResult UpdateCartSession(string Cart)
        {
            var sentCart = JsonConvert.DeserializeObject<List<CartProductDTO>>(Cart);

            var respond = new
            {
                success = false,
                message = "Something went wrong.",
                cart = new List<CartProductDTO>()
            };
            if(HttpContext.Session.GetString("user") == null)
            {
                respond = new
                {
                    success = false,
                    message = "Please log in!",
                    cart = new List<CartProductDTO>()
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
        private CartProductDTO CheckIfInCart(int ProductID, List<CartProductDTO> List)
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
    }
}