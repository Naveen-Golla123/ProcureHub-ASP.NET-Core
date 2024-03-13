using Newtonsoft.Json;

namespace ProcureHub_ASP.NET_Core.Models
{
    public class Lot
    {
        public int _id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<Item> has_item { get; set; }
        public int? EventId { get; set; }
    }
}
