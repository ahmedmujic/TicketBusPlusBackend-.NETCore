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
using UserInfoResponseDTO = AuthService.Models.Dto.Response.UserInfoResponseDTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityModel;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [AllowAnonymous]
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

        [HttpGet("user")]
        public async Task<ActionResult<UserInfoResponseDTO>> GetUserInfo()
        {
            try
            {
                //_logger.LogError("test", "GET:/user");
                var token = Request.Headers.Values;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var result = await _tokenService.GetUserInfoAsync(userId, role).ConfigureAwait(false);
                result.Role = User.FindFirst(ClaimTypes.Role)?.Value;
                return Ok(result);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "GET:/user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("token/revoke")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDTO tokenRequest)
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                tokenRequest.RefreshToken = refreshToken;

                TokenRevocationResponse result = await _tokenService.RevokeTokenAsync(tokenRequest).ConfigureAwait(false);

                if (result.IsError)
                    return BadRequest(IdentityResult.Failed(new IdentityError { 
                        Code = result.Error,
                        Description = null
                    }));

                Response.DeleteTokenCookie();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/token/revoke");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO request)
        {
            try
            {
                var result = await _tokenService.ResetPasswordAsync(request).ConfigureAwait(false);

                if (result.Succeeded)
                    return Ok();

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/password/request-reset");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
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
