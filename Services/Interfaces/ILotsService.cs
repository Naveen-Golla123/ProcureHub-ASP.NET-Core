using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Services.Interfaces
{
    public interface ILotsService
    {
        public Task<List<Lot>> GetAllLots(int eventId);
        public Task<Lot> GetLotById(string id);
        public Task<Lot> SaveLot(Lot lot);
        public Task<bool> DeleteLotById(string id);
        public Task<List<int>> DeleteLot(List<int> id);
        public Task<int> DeleteItem(int id);
    }
}
