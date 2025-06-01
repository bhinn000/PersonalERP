using PersonalERP.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PersonalERP.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                return "Anonymous";  // or throw or handle as needed

            return user.Identity.Name ?? "Unknown";
        }
    }

}


