using Microsoft.AspNetCore.Identity;
using System.Data;

namespace ProcureHub_ASP.NET_Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}
