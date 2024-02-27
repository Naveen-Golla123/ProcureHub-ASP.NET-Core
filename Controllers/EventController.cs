using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService eventService;

        public EventController(IEventService _eventService)
        {
            eventService = _eventService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveEvent([FromBody] Event _event)
        {
            return Ok(await eventService.SaveEvent(_event));
        }

        //[HttpGet]
        //public async Task<IActionResult> IsEventNameAvailable()
        //{

        //}
    }
}
