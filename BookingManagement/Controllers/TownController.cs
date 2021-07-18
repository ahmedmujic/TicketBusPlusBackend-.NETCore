using BookingManagement.Models.DTO.Town;
using BookingManagement.Services.Town.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Controllers
{
    [Route("[controller]")]
    public class TownController : ControllerBase
    {
        private readonly ILogger<TownController> _logger;
        private readonly ITownService _townService;

        public TownController(ILogger<TownController> logger,
            ITownService townService) 
        {
            _logger = logger;
            _townService = townService;

        }

        [HttpGet]
        public async Task<IActionResult> GetTowns([FromQuery] TownRequest request)
        {
            try
            {
                var result = await _townService.GetTownsAsync(request).ConfigureAwait(false);
                var metadata = new
                {
                    result.TotalItems,
                    result.HasNext,
                    result.Limit,
                    result.Offset
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/towns");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadTownsFile([FromQuery] TownRequest request)
        {
            try
            {
                var result = await _townService.GetTownsAsync(request).ConfigureAwait(false);
                return Ok(result);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "POST:/Upload/towns");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
