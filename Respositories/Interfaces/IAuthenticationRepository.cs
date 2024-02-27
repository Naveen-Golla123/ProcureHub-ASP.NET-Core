using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Respositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<bool> RegisterUser(UserDetails userDetails);

        public Task<string> SignIn(string userName, string password);

        public Task<bool> IsEmailAvailable(string email);

        public Task<UserDetails> GetUserDetails(string email);
    }
}
