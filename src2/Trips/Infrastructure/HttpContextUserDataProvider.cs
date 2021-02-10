using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using Trips.Domain.Authorization;

namespace Trips.Infrastructure
{
    internal sealed class HttpContextUserDataProvider : IUserDataProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextUserDataProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        IUser IUserDataProvider.GetUserData()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var name = user.Identity.Name;

            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            Enum.TryParse(
                user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role).Value, 
                out UserRole role);

            return new User
            {
                Name = name,
                Role = role
            };
        }

        internal class User : IUser
        {
            public string Name { get; set; }
            public UserRole Role { get; set; }
        }
    }
}
