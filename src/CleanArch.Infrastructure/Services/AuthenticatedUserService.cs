using CleanArch.Application.Abstractions.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CleanArch.Infrastructure.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _context;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                return _context.HttpContext.User.FindFirstValue("uid");
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _context.HttpContext.User.Identity.IsAuthenticated;
            }
        }

        public List<KeyValuePair<string, string>> Claims
        {
            get
            {
                return _context.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(
                    item.Type,
                    item.Value)).ToList();
            }
        }
    }
}
