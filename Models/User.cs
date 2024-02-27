namespace ProcureHub_ASP.NET_Core.Models
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public bool isBuyer { get; set; }
        public bool isAdmin { get; set; }
        public bool? isApproved { get; set; }
        public string mobile { get; set; }
    }
}
