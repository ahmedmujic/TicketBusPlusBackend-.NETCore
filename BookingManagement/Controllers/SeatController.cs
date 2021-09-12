using BookingManagement.Services.Seat.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly ILogger<SeatController> _logger;

        public SeatController(ISeatService busService,
           ILogger<SeatController> logger)
        {
            _seatService = busService;
            _logger = logger;
        }

        [HttpGet("{busId}")]
        public async Task<ActionResult> GetSeatsByBusId(int busId)
        {
            try
            {
                var result = await _seatService.GetSeatsByBusIdAsync(busId).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/api/Seat/{busId}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
