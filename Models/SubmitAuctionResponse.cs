namespace ProcureHub_ASP.NET_Core.Models
{
    public class SubmitAuctionResponse
    {
        public int Id { get; set; }
        public bool isSubmitted {  get; set; }
        public List<string> errors { get; set; }
    }
}
