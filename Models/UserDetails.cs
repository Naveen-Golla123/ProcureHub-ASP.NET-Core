
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ProcureHub_ASP.NET_Core.Models
{
    public class UserDetails: User
    {
        public string password { get; set; }
        public byte[] hashedPassword {  get; set; }
        public byte[] hashSalt { get; set; }
    }
}
