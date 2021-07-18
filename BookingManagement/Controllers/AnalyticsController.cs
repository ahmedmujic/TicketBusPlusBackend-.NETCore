using BookingManagement.Services.Analytics.Interface;
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
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(IAnalyticsService analyticsService,
            ILogger<AnalyticsController> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        [HttpGet("getSellStat")]
        public async Task<ActionResult> GetRouteStatsAsync()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _analyticsService.GetRouteStatsAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/api/Analytics/getSellStat");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("country-stats")]
        public async Task<ActionResult> GetCountryStats()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _analyticsService.GetCountriesAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/api/Analytics/country-stats");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("monthly-stats")]
        public async Task<ActionResult> GetMonthlyStats()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _analyticsService.GetMonthAnalyticsAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/api/Analytics/country-stats");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
