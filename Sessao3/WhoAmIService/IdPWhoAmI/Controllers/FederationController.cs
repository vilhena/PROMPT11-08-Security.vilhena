using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IdPWhoAmI.Models;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Web;

namespace IdPWhoAmI.Controllers
{
    public class FederationController : Controller
    {
        //
        // GET: /FederationController/

        [Authorize]
        public ActionResult Issue()
        {
            
             // Get the incoming IClaimsIdentity from IPrincipal 
            var callerIdentity = (FormsIdentity) User.Identity;
            var idPClaimses = new List<IdPClaims>();

            var req = WSFederationMessage.CreateFromUri(Request.Url);
            var resp = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(
                req as SignInRequestMessage,
                User,
                new SimpleSecurityTokenService(new SimpleSecurityTokenServiceConfiguration()));


            var sb = new StringBuilder();
            var tw = new StringWriter(sb);

            resp.Write(tw);

            idPClaimses.Add(new IdPClaims()
                                {
                                    Name = "x",
                                    Value = sb.ToString()
                                });

            foreach (var claim in resp.Parameters)
            {
                idPClaimses.Add(new IdPClaims()
                                    {
                                        Name = claim.Key,
                                        Value = claim.Value
                                    });
            }
            return View(idPClaimses);
        }

        [HttpPost]
        public void Issue(bool accepted)
        {
            if (accepted)
            {
                var req = WSFederationMessage.CreateFromUri(Request.Url);
                var resp = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(
                    req as SignInRequestMessage,
                    User,
                    new SimpleSecurityTokenService(new SimpleSecurityTokenServiceConfiguration()));
                resp.Write(Response.Output);

                Response.Flush();
                Response.End();
            }
        }



    }

    public class SimpleSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        public SimpleSecurityTokenServiceConfiguration()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates.OfType<X509Certificate2>().Where(c => c.Subject.Contains("sign")).
                FirstOrDefault();
            this.SigningCredentials = new X509SigningCredentials(certificate);

            this.TokenIssuerName = "https://idp.prompt11.local";
        }
    }

    public class SimpleSecurityTokenService : SecurityTokenService
    {

        public SimpleSecurityTokenService(SimpleSecurityTokenServiceConfiguration simpleSecurityTokenServiceConfiguration) 
            : base(simpleSecurityTokenServiceConfiguration)
        {
            //throw new NotImplementedException();
        }

        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            // Create the scope using the request AppliesTo address and the STS signing certificate
            Scope scope = new Scope(request.AppliesTo.Uri.AbsoluteUri
                                    , SecurityTokenServiceConfiguration.SigningCredentials);
            scope.TokenEncryptionRequired = false;
            scope.ReplyToAddress = "https://www.prompt11.local:8443/";

            return scope;
        }



        /// <summary>
        /// This method returns the claims to be included in the issued token. 
        /// </summary>
        /// <param name="scope">The scope that was previously returned by GetScope method</param>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST</param>
        /// <returns>The claims to be included in the issued token.</returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {


            if (null == principal)
            {
                throw new InvalidRequestException("The caller's principal is null.");
            }



            // Get the incoming IClaimsIdentity from IPrincipal 
            IClaimsIdentity callerIdentity = (IClaimsIdentity)principal.Identity;


            // Create the output IClaimsIdentity
            IClaimsIdentity outputIdentity = new ClaimsIdentity();

            foreach (var claim in callerIdentity.Claims)
            {
                var newClaim = new Claim(claim.ClaimType, claim.Value, claim.ValueType, claim.Issuer);

                outputIdentity.Claims.Add(newClaim);
            }

            return outputIdentity;
        }
    }
}
