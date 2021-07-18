using BookingManagement.Models.DTO.Info.Request;
using BookingManagement.Services.Info.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IInfoService _infoService;
        private readonly ILogger<InfoController> _logger;

        public InfoController(IInfoService infoService,
            ILogger<InfoController> logger)
        {
            _infoService = infoService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailInfo(SendInfoMailDTO request)
        {
            try
            {
                var result = await _infoService.SendInfoEmailAsync(request);
                if (result)
                {
                    return Ok();
                }
                return BadRequest();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "POST:/api/Info");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
