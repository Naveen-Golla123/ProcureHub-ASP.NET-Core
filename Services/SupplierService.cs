using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository supplierRepository;
        public SupplierService(ISupplierRepository _supplierRepo) 
        {
            supplierRepository = _supplierRepo;
        }

        public Task<List<User>> GetSuppliers()
        {
            return supplierRepository.GetSuppliers();
        }
    }
}
