using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace Church.Host.Owin.Core.Authentication
{
    public class ChurchOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult(0);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            if (context.UserName != context.Password)
            {
                context.Rejected();
                return;
            }

            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim("sub", context.UserName));
            id.AddClaim(new Claim("role", "user"));

            context.Validated(id);
        }
    }
}