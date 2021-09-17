using BookingManagement.Models.DTO.Ticket.Request;
using BookingManagement.Services.Ticket.Interface;
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
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ILogger<TicketController> logger, ITicketService ticketService)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> BookTicket([FromBody] BookTicketDTO request)
        {
            try
            {
                request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                request.Email = User.FindFirst(ClaimTypes.Email)?.Value;
                var result = await  _ticketService.BookTicketAsync(request);

                if (result)
                    return Ok();
                else
                    return BadRequest();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/Ticket");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
