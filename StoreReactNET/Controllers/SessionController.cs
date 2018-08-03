using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StoreReactNET.Controllers
{
    [Route("Session/[action]")]
    public class SessionController : Controller
    {
        [HttpGet]
        public ActionResult GetUserSession()
        {
            var respond = new
            {
                isEstablished = false,
                data = ""
            };
            var session = HttpContext.Session.GetString("user");

            if(session != null)
            {
                respond = new
                {
                    isEstablished = true,
                    data = session
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
    }
}