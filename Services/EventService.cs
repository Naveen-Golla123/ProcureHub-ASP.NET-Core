using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;
        public EventService(IEventRepository _eventRepository) 
        { 
            eventRepository = _eventRepository;
        }

        public async Task<bool> SaveEvent(Event _event)
        {
            return await eventRepository.SaveEvent(_event);
        }
    }
}
