using ProcureHub_ASP.NET_Core.Enums;

namespace ProcureHub_ASP.NET_Core.Models
{
    public class SupplierEvent
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public EventStatus StatusCode { get; set; }
        public string Description { get; set; }
        public bool isInvited { get; set; }
        public bool isAdded { get; set; }
        public bool isAccepted { get; set; }
        public bool isRejected { get; set; }
        public User CreatedBy { get; set; } = new User();


    }
}
