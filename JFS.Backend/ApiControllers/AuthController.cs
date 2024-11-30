using JFS.Backend.Enitites;
using Microsoft.AspNetCore.Mvc;
using JFS.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using JFS.Backend.DTOs;

namespace JFS.Backend.ApiControllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            var result = await _authService.LoginAsync(user);
            if (result.Success)
                return Ok(result);
            else return BadRequest(result);
        }

        [Authorize]
        [HttpGet("test-auth1")]
        public async Task<IActionResult> TestAuth()
        {
            return Ok("Ok");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("test-auth2")]
        public async Task<IActionResult> TestAuth2()
        {
            return Ok("");
        }

        [Authorize(Roles = "staff")]
        [HttpGet("test-auth3")]
        public async Task<IActionResult> TestAuth3()
        {
            return Ok("");
        }
    }
}
