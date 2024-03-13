using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Services.Interfaces
{
    public interface ISupplierService
    {
        public Task<List<User>> GetSuppliers();
        public Task<List<User>> GetAddedSuppliers(int eventId);
    }
}
