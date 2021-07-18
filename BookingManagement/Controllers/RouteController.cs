using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Route;
using BookingManagement.Models.DTO.Route.Request;
using BookingManagement.Models.DTO.Route.Response;
using BookingManagement.Services.Town.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IRouteService routeService,
            ILogger<RouteController> logger)
        {
            _routeService = routeService;
            _logger = logger;
        }

       
        [HttpPost("add")]
        public async Task<ActionResult<AddRouteResponseDTO>> AddRoute([FromBody] AddRouteRequestDTO request)
        {
            try
            {
                request.CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _routeService.AddRouteAsync(request);

                return Ok();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "POST:/api/route/add");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<PaginationListResponse<RouteResponseDTO>>> GetAllRoutes([FromQuery] RoutesRequest request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                request.Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var result = await _routeService.GetRoutesAsync(request).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/api/route");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{routeId}")]
        public async Task<ActionResult<PaginationListResponse<RouteResponseDTO>>> GetRouteById(string routeId)
        {
            try
            {
                var result = await _routeService.GetRouteByIdAsync(routeId).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/api/route/{id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("top-selling")]
        public async Task<ActionResult<TopSellingResponseDTO>> GetTopSellingRoutes()
        {
            try
            {
                var result = await _routeService.GetTopSellingRoutesAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/api/route/top-selling");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
