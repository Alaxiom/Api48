using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(Apiv48.App_Start.AuthenticationStartup))]

namespace Apiv48.App_Start
{
    public class AuthenticationStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // May or may not want to do this.  Look at name difference in User.Identity            
            JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();            

            app.UseIdentityServerBearerTokenAuthentication(
                new IdentityServerBearerTokenAuthenticationOptions
                {
                    
                    Authority = "https://localhost:5001",

                    // Explicitly state from where the token will be sourced. Just in case auth cookie middleware added, or existing
                    
                    AuthenticationType = "Bearer",
                    /*
                     * Setting the validation mode to local forces the api to download the public certificate
                     * from identity server, in this repo that would be the following endpoint:
                     * 
                     * https://localhost:5001/.well-known/openid-configuration/jwks
                     * 
                     * and validate the token certificate locally (within the api).
                     * 
                     * Setting as ValidationMode.ValidationEndpoint causes the api to present the token to 
                     * identity server to validate
                     * 
                     * Not setting the ValidationMode, or setting it to ValidationMode.Both causes some heuristic to kick
                     * in whereby the IdServ package within the api determines which of Local or ValidationEndpoint to apply
                     * during that query session, see:
                     * https://stackoverflow.com/questions/36844391/how-useidentityserverbearertokenauthentication-validates-the-jwt-token-with-loc
                     * 
                     * However, there have been reported timeout issues with the both option so would recommend setting this as Local
                     * 
                     */
                    ValidationMode = ValidationMode.Local,
                    
                    RequiredScopes = new[] { "api2" },       
                    
                    RoleClaimType = "role",
                    /* Using the unique id as name, this will push the value from sub into the identity if the
                     * inbound claims are cleared
                     */
                    NameClaimType = "sub"
                });            
        }
    }
}
