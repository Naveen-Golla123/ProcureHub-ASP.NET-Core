using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Respositories.Interfaces
{
    public interface IEventRepository
    {
        public Task<bool> SaveEvent(Event _event);
    }
}
