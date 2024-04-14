namespace ProcureHub_ASP.NET_Core.Models
{
    public class Bid
    {
        public float BidAmount { get; set; }
        public int UserId { get; set; }
        public string SupplierName { get; set; }
        public DateTime BidTime { get; set; }
    }
}
