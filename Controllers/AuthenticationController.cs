
using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Models;
using Newtonsoft.Json.Linq;
using ProcureHub_ASP.NET_Core.Services;
using Microsoft.AspNetCore.Authorization;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController: ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILiveService liveService;

        public AuthenticationController(IAuthenticationService authService, ILiveService _liveservice) 
        {
            _authenticationService = authService;
            liveService = _liveservice;
        }

        [HttpPost(Name = "SignUp")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO registerDTO)
        {
            
            if(await _authenticationService.IsEmailAvailable(registerDTO.email))
            {
                return Ok("Email Already exists");
            }
            UserDetails userDetails = new UserDetails();
            userDetails.name = registerDTO.name;
            userDetails.mobile = registerDTO.mobile;
            userDetails.password = registerDTO.password;
            userDetails.isBuyer = registerDTO.isBuyer;
            userDetails.isAdmin = registerDTO.isAdmin;
            userDetails.email = registerDTO.email;

            return Ok(await _authenticationService.RegisterUser(userDetails));
        }

        [HttpPost(Name ="Login")]
        public async Task<IActionResult> Login(UserDTO user)
        {
              
            return Ok(await _authenticationService.SignIn(user.email, user.password));
        }

        //[HttpPost]
        //public async Task<IActionResult> SignIn([FromBody] UserDetails)
        //{

        //}

        [HttpPost(Name = "CheckEmailAvailability")]
        public async Task<bool> IsEmailAvailable(CheckEmailAvailability checkEmailAvailable)
        {
            liveService.LoadData(128);
            return await _authenticationService.IsEmailAvailable(checkEmailAvailable.Email);
        }

        [Authorize]
        [HttpGet(Name ="GetData")]
        public Task<List<int>> GetData()
        {
            Console.WriteLine(User.Identity);
            return Task.FromResult(new List<int> { 1, 2, 3, 4 });
        }
    }
}
