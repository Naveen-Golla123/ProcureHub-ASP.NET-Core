using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Respositories.Interfaces
{
    public interface ILotsRepository
    {
        public Task<List<Lot>> GetAllLots(int eventId);
        public Task<Lot> GetLotById(string id);
        public Task<Lot> CreateLot(Lot lot);
        public Task<Lot> CreateItems(Lot lot);
        public Task<Lot> UpdateItems(Lot lot);
        public Task<Lot> UpdateLot(Lot lot);
        public Task<bool> DeleteLotById(string id);
        public Task<List<int>> DeleteLot(List<int> id);
        public Task<int> DeleteItem(int id);
    }
}
