using AuthService.Interfaces;
using AuthService.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        public ILogger<RegisterController> _logger;

        private readonly IRegistrationService _registrationService;

        public RegisterController(IRegistrationService registrationService,
             ILogger<RegisterController> logger)
        {
            _logger = logger;
            _registrationService = registrationService; 
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegistrationDto registration)
        {
            try
            {
                IdentityResult result = await _registrationService.Register(user)
                                                          .ConfigureAwait(false);

                if (result.Succeeded)
                    return Ok(result);
                return Conflict(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/register");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost("confirmEmail/{userId}/{token}")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId is null || token is null)
                return BadRequest(new { message= "userId and token are required" });
            try
            {
                var result = await _registrationService.ConfirmEmail(userId, token);
                if (result)
                    return NoContent();
                return BadRequest(new { message = "Invalid id or token" });
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Something went wrong");
            }
        }
    }
}
