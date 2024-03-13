using ProcureHub_ASP.NET_Core.Controllers;
using ProcureHub_ASP.NET_Core.Enums;

namespace ProcureHub_ASP.NET_Core.Models
{
    public class Event
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int BusinessType { get; set; }
        public string? description { get; set; } = string.Empty;
        public string? Startdate { get; set; }
        public string? Enddate { get; set; }
        public string? Starttime { get; set; }
        public string? Endtime { get; set; }
        public EventStatus statusCode { get; set; }
        public List<Lot> lots { get; set; }
        public List<User> suppliers { get; set; }
    }
}
