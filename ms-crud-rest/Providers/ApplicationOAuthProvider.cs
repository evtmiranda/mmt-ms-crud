using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ms_crud_rest.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext c)
        {
            c.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext c)
        {
            // Aqui você deve implementar sua regra de autenticação
            if (c.UserName == "MaRmITEXuSerREST" && c.Password == "ConseguiConfigurarOToken090420171008654")
            {
                Claim claim1 = new Claim(ClaimTypes.Name, c.UserName);
                Claim[] claims = new Claim[] { claim1 };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(
                       claims, OAuthDefaults.AuthenticationType);
                c.Validated(claimsIdentity);
            }

            return Task.FromResult<object>(null);
        }
    }
}