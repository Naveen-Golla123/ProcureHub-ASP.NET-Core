namespace ProcureHub_ASP.NET_Core.Models
{
    public class RankDetails
    {
        public int Rank {  get; set; }
        public User Supplier { get; set; }
        public float TotalBid { get; set; }

    }
}
