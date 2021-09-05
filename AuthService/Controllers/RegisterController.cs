using AuthService.Constants.Errors;
using AuthService.Interfaces;
using AuthService.Models.Dto;
using AuthService.Models.Dto.Request;
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

        [HttpPost("user")]
        public async Task<ActionResult> RegisterUser([FromBody]UserDto user)
        {
            try
            {
                IdentityResult result = await _registrationService.RegisterUserAsync(user)
                                                          .ConfigureAwait(false);
                if (result.Succeeded)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/register");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost("confirm")]
        public async Task<ActionResult<IdentityResult>> ConfirmEmail(ActivateAccountRequestDTO request)
        {
            try
            {
                var errorDescriber = new IdentityErrorDescriber();
                var result = await _registrationService.ActivateAccountAsync(request);

                if (result.Succeeded)
                    return Ok();
               
                return BadRequest(IdentityResult.Failed(errorDescriber.InvalidToken()));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "POST:/confirmEmail");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        
        [HttpPost("resend/{id}")]
        public async Task<IActionResult> ResendEmail(string id)
        {
            try
            {
                var result = await _registrationService.ResendActivationMailAsync(id);

                if (result.Succeeded)
                    return Ok();

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/confirmEmail");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
