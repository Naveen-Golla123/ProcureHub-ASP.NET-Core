namespace ProcureHub_ASP.NET_Core.Models
{
    public class Eventdetails
    {
        public Event EventInfo { get; set; }
        public Dictionary<string, Lot> Lots { get; set; }
        public Dictionary<string, SupplierLots> Suppliers { get; set; }
        public Dictionary<string,RankDetails> Ranks { get; set; }
        public List<Bid> TopBiders { get; set; }
    }
}
