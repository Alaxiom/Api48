using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;

namespace Apiv48.Controllers
{
    public class IdentityController : Controller
    {      
        [Authorize]
        public void Index()
        {            
            if(User.Identity is ClaimsIdentity)
            {
                var claims = ((ClaimsIdentity)User.Identity).Claims;                
            }
        }
    }
}