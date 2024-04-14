using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Any;

namespace ProcureHub_ASP.NET_Core.Models
{
    public class SupplierLots : User
    {
        //public int id {  get; set; }
        //public string name { get; set; }
        //public string email { get; set; }
        //public bool isBuyer {  get; set; }
        //public bool isAdmin { get; set; }
        //public bool isApproved { get; set; }
        //public bool isSupplier { get; set; }
        //public string mobile { get; set; }
        public Dictionary<string, Lot> supplierLots { get; set; }
        public float CurrentBid { get; set; }
        public List<Bid> BidTracker {  get; set; }
        public int EventId { get; set; }
    }
}
