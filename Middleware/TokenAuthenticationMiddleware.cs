using apiAspNet.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace apiAspNet.Middleware
{
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request is for POST /api/users or for Swagger UI
            if ((context.Request.Path.StartsWithSegments("/api/users") && context.Request.Method.Equals("POST")) ||
                context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            // If not, check for Bearer token in the request header
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Validate the token
            bool isValidToken = ValidateToken(context, token);

            if (!isValidToken)
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            await _next(context);
        }

        private bool ValidateToken(HttpContext context, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                // Token is empty or missing
                return false;
            }

            // Resolve the ApplicationDbContext from the HttpContext RequestServices
            var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();

            // Check if the token exists in the PersonalAccessToken table
            var tokenExists = dbContext.PersonalAccessTokens.Any(t => t.Token == token);

            // You may also want to check if the token is associated with a valid user
            // For example:
            // var tokenIsValid = dbContext.PersonalAccessTokens.Any(t => t.Token == token && t.IsValid);

            return tokenExists;
        }
    }

    public static class TokenAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenAuthenticationMiddleware>();
        }
    }
}
