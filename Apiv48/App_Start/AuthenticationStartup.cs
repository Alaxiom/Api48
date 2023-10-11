using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Apiv48.App_Start.AuthenticationStartup))]

namespace Apiv48.App_Start
{
    public class AuthenticationStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(
                new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = "https://localhost:5001",
                    ValidationMode = ValidationMode.Local,
                    RequiredScopes = new[] { "api2" }
                });            
        }
    }
}
