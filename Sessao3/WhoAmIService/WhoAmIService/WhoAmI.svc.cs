using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.IdentityModel.Claims;

namespace WhoAmIService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WhoAmI" in code, svc and config file together.
    public class WhoAmI : IWhoAmI
    {
        public string Get()
        {
            var ident = Thread.CurrentPrincipal as IClaimsPrincipal;
            var claims = ident.Identities[0].Claims;
            var sb = new StringBuilder();

            foreach (var claim in claims)
            {
                sb.AppendFormat("{0} = {1}", claim.ClaimType, claim.Value).AppendLine();
            }

            return sb.ToString();
        }
    }
}
