using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Station.Response;
using BookingManagement.Services.Station.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationService;
        private readonly ILogger<StationController> _logger;

        public StationController(IStationService stationService,
            ILogger<StationController> logger)
        {
            _stationService = stationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationListResponse<StationResponseDTO>>> GetStations([FromQuery] PaginationRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _stationService.GetStationsAsync(request).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/api/station");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
