using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Enums;
using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Respositories.Interfaces
{
    public interface IEventRepository
    {
        public Task<Event> UpdateEvent(Event _event);
        public Task<Event> CreateEvent(Event _event);
        public Task<List<Event>> GetAllEvents();
        public Task<Event> GetEventById(int id);
        public Task<bool> AddSuppliers(List<int> supplierIds, int eventId);
        public Task<Event> GetEventInfo(int eventId);
        public Task<bool> ChangeAuctionStatus(int eventId, EventStatus eventStatus);
        public Task<List<SupplierEvent>> GetInvitedEvents();
        public Task<int> AcceptEvent(int eventId);
        public Task<int> RejectEvent(int eventId);
    }
}
