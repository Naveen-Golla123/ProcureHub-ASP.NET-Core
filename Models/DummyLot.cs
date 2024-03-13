using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ProcureHub_ASP.NET_Core.Models
{
    public class DummyLot
    {
        public string _type { get; set; }
        public DummyItem[] has_item { get; set; }
        public int _id { get; set; }
        public string description { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
