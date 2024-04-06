using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Middlewares
{
    public class SecurityHeadersMiddleware
    {

        private readonly RequestDelegate next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("X-Frame-Options", "DENY");
            httpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            httpContext.Response.Headers.Add(
                "Content-Security-Policy",
                "default-src 'self'; " +
                "font-src 'self'; " +
                "style-src 'self'; " +
                "frame-src 'self';" +
                "connect-src 'self';");
            httpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");


            await next(httpContext);
        }

    }
}
