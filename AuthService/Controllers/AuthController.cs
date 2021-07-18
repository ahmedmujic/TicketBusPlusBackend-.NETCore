using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Helpers;
using AuthService.Models.Dto;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUserAsync(AuthenticationDto authenticationDto)
        {
            try
            {
                var result = await _authService.AuthenticateUserAsync(authenticationDto, HelpersFunctions.GetIpAddress(Request, HttpContext));

                if (result is null)
                    return BadRequest("Wrong username or password");
                HelpersFunctions.SetTokenCookie(result.RefreshToken, Response);

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Something went bad");
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken is null)
                return BadRequest("Refresh token is not present");
            try
            {
                var response = await _authService.RefreshTokenAsync(refreshToken, HelpersFunctions.GetIpAddress(Request, HttpContext));
                if (response is null)
                    return Unauthorized(new { message = "Invalid token" });

                HelpersFunctions.SetTokenCookie(refreshToken, Response);

                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Something went wrong");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("nesto")]
        public async Task<IActionResult> GetNesto()
        {
            return Ok("dd");
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequestDto revokeTokenRequest)
        {
            var token = revokeTokenRequest.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new {message = "Token is required"});
            try
            {
                var result = await _tokenService.RevokeToken( token ,HelpersFunctions.GetIpAddress(Request, HttpContext));

                if (!result)
                    return NotFound("Token not found");

                return Ok(new {message = "Token revoked"});

            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong revoking token");
            }
        }
    }
}
