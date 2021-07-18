using BookingManagement.Models.DTO.Seat.Response;
using BookingManagement.Services.Seat.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Seat
{
    public class SeatService : ISeatService
    {
        private readonly BookingManagementDbContext _dbContext;
        private readonly ILogger<SeatService> _logger;

        public SeatService(BookingManagementDbContext dbContext,
            ILogger<SeatService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<SeatResponseDTO>> GetSeatsByBusIdAsync(int busId)
        {

            try
            {
                var result = await _dbContext.Seats
                    .AsNoTracking()
                    .Where(s => s.BusId == busId)
                    .Select(s => new SeatResponseDTO { 
                        Checked = s.Checked,
                        BusId = s.BusId,
                        Id = s.Id,
                        SeatCode = s.SeatCode
                    })
                    .ToListAsync();

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetSeatsByBusIdAsync));
                throw;
            }
        }
    }
}
