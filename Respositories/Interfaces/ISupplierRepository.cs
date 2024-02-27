using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Respositories.Interfaces
{
    public interface ISupplierRepository
    {
        public Task<List<User>> GetSuppliers();
    }
}
