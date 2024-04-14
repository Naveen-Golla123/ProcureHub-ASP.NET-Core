using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ProcureHub_ASP.NET_Core.Filters
{
    
    public class ExtractInfoFilter : Attribute ,IActionFilter, IHubFilter
    {
        private IUserContext _userContext;
        public ExtractInfoFilter() 
        {
            //_userContext = _context;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine(context);
            IUserContext service = context.HttpContext.RequestServices.GetService<IUserContext>();
            service.SetEmail("Golla Naveen");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            IUserContext service = context.HttpContext.RequestServices.GetService<IUserContext>();
            string authHeader = context.HttpContext.Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", string.Empty); 
            if (authHeader != null)
            {
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadJwtToken(authHeader);
                if (token != null)
                {
                    service.SetEmail(token.Claims.First(c => c.Type == "email").Value);
                    service.SetUserId(Int32.Parse(token.Claims.First(c => c.Type == "userId").Value));
                }
            }
        }
    }
}


//MATCH(supplier: user) where ID(supplier) = 19
//match(event:Event)  where Id(event) = 8
//with supplier, event
//merge (supplier)-[r: INVITED_TO{accepted:true}]->(event) 
// return supplier, event