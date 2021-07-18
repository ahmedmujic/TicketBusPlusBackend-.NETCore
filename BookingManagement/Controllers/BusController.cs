using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Bus.Response;
using BookingManagement.Services.Bus.Interface;
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
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusController : ControllerBase
    {
        private readonly IBusService _busService;
        private readonly ILogger<BusController> _logger;

        public BusController(IBusService busService,
            ILogger<BusController> logger)
        {
            _busService = busService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationListResponse<BusResponseDTO>>> GetAllBusses([FromQuery] PaginationRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _busService.GetBussesAsync(request).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/api/bus");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBus([FromBody] AddBusRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _busService.AddBusAsync(request).ConfigureAwait(false);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/api/bus");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("csv")]
        public async Task<IActionResult> AddBusCsv([FromForm] IFormFile file)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _busService.AddBusFromCsvAsync(file, userId).ConfigureAwait(false);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/api/bus");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
