using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Services.Interfaces
{
    public interface ILiveService
    {
        public Task<bool> LoadData(int eventId);
        public Task<Eventdetails> GetSupplierDashBoardData(int eventId);
        public Task<Eventdetails> PlaceBid(SupplierLots supplierLot);
    }
}
