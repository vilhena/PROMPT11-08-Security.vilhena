using System.Collections.Generic;
using System.Web.Mvc;
using MvcFederated.Models;

namespace MvcFederated.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        [Authorize]
        [RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")]
        [RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")]
        public ActionResult About()
        {
            return View();
        }
    }
}
