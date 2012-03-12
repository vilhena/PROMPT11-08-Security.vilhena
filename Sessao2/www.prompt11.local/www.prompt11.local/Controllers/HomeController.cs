using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;

namespace www.prompt11.local.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public string Index()
        {
            var ident = this.User as IClaimsPrincipal;
            var emailClaim = ident.Identities[0].Claims;
            var sb = new StringBuilder();

            foreach (var c in emailClaim)
            {

                sb.Append("<p>")
                    .Append("<BR />ValueType = ").Append(c.ValueType)
                    .Append("<BR />ClaimType = ").Append(c.ClaimType)
                    .Append("<BR />Value = ").Append(c.Value)
                    .Append("<BR />Issuer = ").Append(c.Issuer)
                    .Append("<p>");
            }
            
            return "Hi there <br/>" + sb.ToString();

        }

    }
}
