using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.TokenServices
{
    public class TokenService
    {
        public long UserID { get; set; }
        public string UserEmail { get; set; } = "";
        public string UserName { get; set; } = "";
        // public string Role { get; set; } = "";
        public TokenService(IHttpContextAccessor httpContext)
        {
            if (httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (httpContext.HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    if (claims.Any())
                    {
                        UserEmail = identity.FindFirst("Email").Value;
                        //Role = identity.FindFirst("Type").Value;
                        UserID = Convert.ToInt64(identity?.FindFirst("ID")?.Value);
                        UserName = identity?.FindFirst("UserName")?.Value;
                    }

                }
            }
        }
    }
}
