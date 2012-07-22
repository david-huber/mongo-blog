using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mongo.Blog.Controllers
{
    public class CookieController : Controller
    {

        public ActionResult Index()
        {
            var messages = new List<string>();
            var cookie = Request["DumbCookie"];
            if (cookie != null)
            {
                messages.Add("Request has the cookie");
            }
            else
            {
                messages.Add("Request doesn't have the cookie!");
                Response.Cookies.Add(new HttpCookie("DumbCookie", "DumbValue"));
                cookie = Request["DumbCookie"];
                if (cookie != null)
                {
                    messages.Add("...and now it does? What the heck, ASP.NET dev team?");                    
                }
            }

            return View(messages);
        }

    }
}