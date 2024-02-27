using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Services.Interfaces
{
    public interface IEventService
    {
        public Task<bool> SaveEvent(Event _event);
    }
}
