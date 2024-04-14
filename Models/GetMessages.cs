using Microsoft.AspNetCore.Mvc;

namespace ProcureHub_ASP.NET_Core.Models
{
    public interface GetMessages
    {
        public int eventId { get; set; }
        public int otherUserId { get; set; }

    }
}
