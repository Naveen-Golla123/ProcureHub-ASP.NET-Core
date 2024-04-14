namespace ProcureHub_ASP.NET_Core.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string date { get; set; }
        public int SentBy { get; set; }
        public int SentTo { get; set; }
        public bool IsSent { get; set; }
        public int EventId { get; set; }
     }
}
