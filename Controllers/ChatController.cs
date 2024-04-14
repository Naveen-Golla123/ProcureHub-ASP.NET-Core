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
    public class ChatController : ControllerBase
    {
        private readonly IChatService _iChatService;
        public ChatController(IChatService chatService)
        {
             _iChatService = chatService;
        }

        [HttpGet("GetAllMessages/{eventId}/{userId}")]
        public async Task<IActionResult> GetAllMessages(int eventId, int userId) 
        {
            return Ok(await _iChatService.GetMessages(eventId, userId));
        }
    }
}
