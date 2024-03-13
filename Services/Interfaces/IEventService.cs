using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Enums;
using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Services.Interfaces
{
    public interface IEventService
    {
        public Task<Event> UpdateEvent(Event _event);
        public Task<Event> CreateEvent(Event _event);
        public Task<List<Event>> GetAllEvents();
        public Task<Event> GetEventById(int id);
        public Task<bool> AddSuppliers(List<int> supplierIds, int eventId);
        public Task<SubmitAuctionResponse> SubmitAuction(int eventId);
        public Task<bool> ChangeAuctionStatus(int eventId, EventStatus eventStatus);
    }
}
