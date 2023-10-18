using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Apiv48.Controllers
{
    public class IdentityController : ApiController
    {
        [Authorize]
        public List<Thing> Get()
        {
            if (User.Identity is ClaimsIdentity)
            {                
                var claims = ((ClaimsIdentity)User.Identity).Claims;

                return (from c in claims select new Thing { Type = c.Type, Value = c.Value }).ToList();                     
            }

            return new List<Thing>();
        }
    }

    public class Thing
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}