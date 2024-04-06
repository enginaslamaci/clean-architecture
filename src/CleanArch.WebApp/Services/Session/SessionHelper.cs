using CleanArch.Application.Helpers;
using CleanArch.WebApp.Extensions;

namespace CleanArch.WebApp.Services.Session
{
    public static class SessionHelper
    {
        private static HttpContext _httpContext => ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext;

        public static SessionUser CurrentUser
        {
            get { return _httpContext.Session.GetObject<SessionUser>("SessionUser") ?? new SessionUser(); }
            set { _httpContext.Session.SetObject("SessionUser", value); }
        }

        public static bool IsAuthenticated
        {
            get
            {
                if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.ID)) { return true; }
                else { return false; }
            }
        }

        public static async Task<string> GetToken()
        {
            if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.Token))
                return await Task.FromResult(CurrentUser.Token);

            return await Task.FromResult(string.Empty);
        }

    }
}
