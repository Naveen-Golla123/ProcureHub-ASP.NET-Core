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

        public async Task Invoke(HttpContext context, RequestDelegate next)
        {

            
            await _next(context);
        }
    }
}
