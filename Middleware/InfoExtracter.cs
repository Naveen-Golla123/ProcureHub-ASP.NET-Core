using ProcureHub_ASP.NET_Core.Helper;
using System.IdentityModel.Tokens.Jwt;

namespace ProcureHub_ASP.NET_Core.Middleware
{
    public class InfoExtracter
    {
        private readonly RequestDelegate _next;
        private readonly IUserContext userContext;

        public InfoExtracter(RequestDelegate next, IUserContext _userContext) 
        {
            _next = next;
            userContext = _userContext;
        }
        

        public async Task InvokeAsync(HttpContext context)
        {
            string authHeader = context.Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", string.Empty);
            if (authHeader != null)
            {
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadJwtToken(authHeader);
                if (token != null)
                {
                    userContext.SetEmail(token.Claims.First(c => c.Type == "email").Value);
                }
            }
            await _next(context);
        }
    }
}
