using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class LotsService : ILotsService
    {
        private readonly ILotsRepository _lotsRepository;
        public LotsService(ILotsRepository lotsRepository) 
        {
            _lotsRepository = lotsRepository;
        }

        public Task<bool> DeleteLotById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Lot>> GetAllLots(int eventId)
        {
            List <Lot> lots = await _lotsRepository.GetAllLots(eventId);
            foreach(Lot lot in lots) 
            {   
                float totalPrice = 0;
                foreach(Item item in lot.has_item)
                {
                    totalPrice += item.basePrice * item.quantity;
                }
                lot.TotalPrice = totalPrice;
            }
            return lots;
        }

        public Task<Lot> GetLotById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Lot> SaveLot(Lot lot)
        {

            if(lot._id == null || lot._id == 0)
            {
                return await this._lotsRepository.CreateLot(lot);
            }

            Lot lotWithNewItems = new Lot
            {
                _id = lot._id,
                name = lot.name,
                description = lot.description,
                has_item = new List<Item>()
            };

            Lot lotWithExistingItems = new Lot
            {
                _id = lot._id,
                name = lot.name,
                description = lot.description,
                has_item = new List<Item>()
            };

            
            foreach(Item item in lot.has_item)
            {
                if(item._id == 0)
                {
                    lotWithNewItems.has_item.Add(item);
                }else
                {
                    lotWithExistingItems.has_item.Add(item);
                }
            }
            await _lotsRepository.CreateItems(lotWithNewItems);
            return await _lotsRepository.UpdateItems(lotWithExistingItems);
        }

        public async Task<List<int>> DeleteLot(List<int> ids)
        {
            return await _lotsRepository.DeleteLot(ids);
        }

        public async Task<int> DeleteItem(int id)
        {
            return await _lotsRepository.DeleteItem(id);
        }
    }
}
