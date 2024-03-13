using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Respositories.Interfaces
{
    public interface ISupplierRepository
    {
        public Task<List<User>> GetSuppliers();
        public Task<bool> AddSuppliersToEvent();
        public Task<bool> RemoveSupplierFromEvent();
        public Task<bool> TriggerInviteToSupplier();
        public Task<List<User>> GetAddedSuppliers(int eventId);
    }
}
