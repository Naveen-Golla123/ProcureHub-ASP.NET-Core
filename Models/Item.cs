namespace ProcureHub_ASP.NET_Core.Models
{
    public class Item
    {
        public int _id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float basePrice { get; set; }
        public int quantity { get; set; }
    }
}
