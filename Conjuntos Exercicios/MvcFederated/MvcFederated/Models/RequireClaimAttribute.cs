using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;

namespace MvcFederated.Models
{
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

            if (contains == 0)
                throw new RequiredClaimException(Claim);
        }
    }
}