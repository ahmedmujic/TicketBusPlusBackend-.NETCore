using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AuthService.Helpers
{
    public static class HelpersFunctions
    {
        
        public static string GetIpAddress(HttpRequest request, HttpContext httpContext)
        {
            if (request.Headers.ContainsKey("X-Forwarded-For"))
                return request.Headers["X-Forwarded-For"];
            return httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        public static void SetTokenCookie(string token, HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
