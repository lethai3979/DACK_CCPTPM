using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenService;

        public AuthenticationController(AuthenticationService service)
        {
            _authenService = service;
        }

        [HttpPost("Login")]
        public async Task<string> Login(LoginVM loginViewModel)
        {
            var result = await _authenService.LoginAsync(loginViewModel);
            return result;
        }

        [HttpPost("SignUp")]
        public async Task<IdentityResult> SignUp(SignUpVM signUpViewModel)
        {
            var result = await _authenService.SignUpAsync(signUpViewModel);
            return result;
        }
    }
}
