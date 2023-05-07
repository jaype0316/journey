using Journey.Core;
using Journey.Core.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Journey.Api.Middleware
{
    public class OAuthMiddleware
    {
        private readonly RequestDelegate next;

        public OAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var bearerToken = context.Request.Headers["Authorization"].ToString();
            var jwt = bearerToken.Replace("Bearer", "").Trim();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(jwt);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userId))
            {
                return;
            }

            if (!UserContextCache.Contains(userId))
            {
                UserContextCache.Set(userId, new UserContext() { Email = email, UserId = userId });
            }

            await next.Invoke(context);
        }
    }
}
