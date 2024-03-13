using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Filters;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Services;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExtractInfoFilter]
    public class SupplierController : ControllerBase
    {
        private ISupplierService _suppplierService;
        public SupplierController(ISupplierService supplierService) 
        {
            _suppplierService = supplierService;
        }

        [HttpGet]
        public async Task<List<User>> GetSuppliers()
        {
            return await _suppplierService.GetSuppliers(); 
        }

        [HttpGet("getaddedsuppliers/{eventId}")]
        public async Task<IActionResult> GetAddedSuppliers(int eventId)
        {
            return Ok(await _suppplierService.GetAddedSuppliers(eventId));
        }
    }
}
