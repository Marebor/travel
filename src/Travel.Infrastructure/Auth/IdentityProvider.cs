using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using Travel.Common.Auth;

namespace Travel.Infrastructure.Auth
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public IdentityProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Identity GetIdentity()
        {
            string username = httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            string role = httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            return new Identity(username, role);
        }
    }
}
