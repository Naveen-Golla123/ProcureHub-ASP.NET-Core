using ProcureHub_ASP.NET_Core.Controllers;

namespace ProcureHub_ASP.NET_Core.Models
{
    public class Event
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int BusinessType { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateOnly? Startdate { get; set; }
        public DateOnly? Enddate { get; set; }
        public TimeOnly? Starttime { get; set; }
        public TimeOnly? Endtime { get; set; }
        public Lot[]? Lots { get; set; }
    }
}
