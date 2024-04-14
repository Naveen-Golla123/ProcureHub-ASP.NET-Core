using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Filters;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [ExtractInfoFilter]
    public class LiveController : ControllerBase
    {
        private readonly ILiveService liveservice;
        public LiveController(ILiveService _iLiveService) 
        {
            liveservice = _iLiveService;
        }

        [HttpGet("GetSupplierDashBoardData/{eventId}")]
        public async Task<IActionResult> GetSupplierDashBoardData(int eventId)
        {
            return Ok(await liveservice.GetSupplierDashBoardData(eventId));
        }
    }
}
