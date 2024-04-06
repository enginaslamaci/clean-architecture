using CleanArch.WebApp.Services.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanArch.WebApp.Filters
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private string[] roles;

        public AuthorizeAttribute(params string[] _roles)
        {
            roles = _roles ?? throw new ArgumentNullException(nameof(roles));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!SessionHelper.IsAuthenticated)
            {
                context.Result = new RedirectResult("/login");
                return;
            }

            var hasRole = roles.Contains(SessionHelper.CurrentUser.Role);
            if (roles.Length > 0 && !hasRole)
            {
                context.Result = new RedirectResult("/unuthorized");
                return;
            }
        }
    }
}
