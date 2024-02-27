namespace ProcureHub_ASP.NET_Core.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Item[] Items { get; set; }
    }
}
