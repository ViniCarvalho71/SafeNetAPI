using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using SafeNetAPI.Models;
using System.Security.Claims;

namespace SafeNetAPI.Services.Token
{
    public class ApiTokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiTokenAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<UserModel> userManager)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                var token = authHeader.ToString().Replace("Bearer ", "");

                var users = userManager.Users.ToList();
                foreach (var user in users)
                {
                    var storedToken = await userManager.GetAuthenticationTokenAsync(user, "SafeNetAPI", "PersonalAccessToken");

                    if (storedToken == token)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                        var identity = new ClaimsIdentity(claims, "ApiToken");
                        var principal = new ClaimsPrincipal(identity);
                        context.User = principal;

                        break;
                    }
                }
            }

            await _next(context);
        }
    }
}
