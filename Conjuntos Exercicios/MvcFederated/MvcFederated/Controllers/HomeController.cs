using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;

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

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequireClaimAttribute : ActionFilterAttribute
    {
        public string Claim { get; set; }

        public RequireClaimAttribute(string claim)
        {
            Claim = claim;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var ident = HttpContext.Current.User as IClaimsPrincipal;
            var contains = ident.Identities[0].Claims.Where(c=>c.ClaimType==Claim).Count();
        }
    }
}
