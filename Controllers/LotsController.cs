using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Filters;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    [ExtractInfoFilter]
    public class LotsController : ControllerBase
    {
        public readonly ILotsService _lotsService;

        public LotsController(ILotsService lotsService)
        {
            _lotsService = lotsService;
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetAllLots(int eventId)
        {
            return Ok(await _lotsService.GetAllLots(eventId));
        }

        [HttpGet(Name = "GetLoyById")]
        public async Task<IActionResult> GetLotById()
        {
            return Ok(true);
        }

        [HttpPost(Name = "SaveLot")]
        public async Task<IActionResult> SaveLot([FromBody] Lot _lot)
        {
            return Ok(await _lotsService.SaveLot(_lot));
        }

        [HttpPost("deletelot")]
        public async Task<IActionResult> DeleteLot([FromBody] List<int> ids)
        {
            return Ok(await _lotsService.DeleteLot(ids));
        }

        [HttpGet("deleteitem/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            return Ok(await _lotsService.DeleteItem(id));
        }

    }
}
