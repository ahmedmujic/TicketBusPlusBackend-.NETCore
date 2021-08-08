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
using AuthService.Models.Dto.Request;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AuthService.Models.Dto.Response;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService,
            ILogger<AuthController> logger)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] TokenRequestDTO tokenRequest)
        {
            try
            {
                TokenResponse result = await _tokenService.CreateTokenAsync(tokenRequest).ConfigureAwait(false);

                if (result is null)
                    return BadRequest();

                if (result.IsError)
                    return BadRequest(IdentityResult.Failed(new IdentityError
                    {
                        Code = result.Error,
                        Description = result.ErrorDescription
                    }));

                Response.SetTokenCookie(result.RefreshToken);

                return Ok(new TokenResponseDTO { 
                    ExpiresIn = result.ExpiresIn,
                    Token = result.AccessToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/token");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("token/refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO tokenRequest)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken is null)
                return BadRequest("Refresh token is not present");
            else
                tokenRequest.RefreshToken = refreshToken;
             try
            {
                TokenResponse result = await _tokenService.RefreshTokenAsync(tokenRequest).ConfigureAwait(false);

                if (result.IsError)
                    return BadRequest(IdentityResult.Failed(new IdentityError
                    {
                        Code = result.Error,
                        Description = result.ErrorDescription
                    }));

                Response.SetTokenCookie(result.RefreshToken);

                return Ok(new TokenResponseDTO { 
                    ExpiresIn= result.ExpiresIn,
                    Token = result.AccessToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/refresh");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("token/revoke")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDTO tokenRequest)
        {
            try
            {
                TokenRevocationResponse result = await _tokenService.RevokeTokenAsync(tokenRequest).ConfigureAwait(false);

                if (result.IsError)
                    return BadRequest(IdentityResult.Failed(new IdentityError { 
                        Code = result.Error,
                        Description = null
                    }));

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/token/revoke");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("password/request-reset")]
        public async Task<IActionResult> RequestResetPassword(RequestResetPasswordDTO request)
        {
            try
            {
                var result = await _tokenService.RequestResetPassswordAsync(request).ConfigureAwait(false);

                if (result.Succeeded)
                    return Ok();

                return BadRequest(result);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "POST:/password/request-reset");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("password/token-validate")]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityResult>> ValidateResetPasswordTokenAsync([FromBody] PasswordTokenValidationDTO request)
        {
            try
            {
                var result = await _tokenService.ValidateResetPasswordTokenAsync(request).ConfigureAwait(false);

                if (result.Succeeded)
                    return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/password/token-validate");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
