using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Services
{
    public interface IAuthenticationService
    {
        public Task<bool> RegisterUser(UserDetails userDetails);

        public Task<Object> SignIn(string userName, string password);

        public Task<bool> IsEmailAvailable(string email);

        
    }
}
