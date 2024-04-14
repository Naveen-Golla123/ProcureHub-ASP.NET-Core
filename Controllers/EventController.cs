using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using System.Reflection.PortableExecutable;
using ProcureHub_ASP.NET_Core.Filters;

namespace ProcureHub_ASP.NET_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExtractInfoFilter]
    public class EventController : ControllerBase
    {
        private readonly IEventService eventService;
        private IUserContext context_;

        public EventController(IEventService _eventService, IUserContext context)
        {
            eventService = _eventService;
        }

        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] Event _event)
        {
            return Ok(await eventService.CreateEvent(_event));
            
        }

        [HttpPost("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] Event _event)
        {
            return Ok(await eventService.UpdateEvent(_event));
        }

        [HttpGet("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents()
        {
            var str = Request.Headers.Authorization;
            return Ok(await eventService.GetAllEvents());
        }

        [HttpGet("GetEventById/{eventId}")]
        public async Task<IActionResult> GetEventId(int eventId)
        {
            return Ok(await eventService.GetEventById(eventId));
        }

        [HttpPost("AddSupplier/{eventId}")]
        public async Task<IActionResult> AddSuppliers(List<int> supplierIds,int eventId)
        {
            return Ok(await eventService.AddSuppliers(supplierIds, eventId));
        }

        [HttpPost("SubmitAuction/{eventId}")]
        public async Task<IActionResult> SubmitAuction(int eventId)
        {
            return Ok(await eventService.SubmitAuction(eventId));
        }

        [HttpGet("GetInvitedSuppliers")]
        public async Task<IActionResult> GetInvitedEvents()
        {
            return Ok(await eventService.GetInvitedEvents());
        }

        [HttpGet("")]
        public async Task<IActionResult> TestHangfire()
        {
            return Ok(await eventService.TestHangfire());
        }

        [HttpPost("AcceptEvent/{eventId}")]
        public async Task<IActionResult> AcceptEvent(int eventId)
        {
            return Ok(await eventService.AcceptEvent(eventId));
        }

        [HttpPost("RejectEvent/{eventId}")]
        public async Task<IActionResult> RejectEvent(int eventId)
        {
            return Ok(await eventService.RejectEvent(eventId));
        }
    }
}
