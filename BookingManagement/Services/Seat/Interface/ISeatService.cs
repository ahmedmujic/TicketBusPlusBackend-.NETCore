using BookingManagement.Models.DTO.Seat.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Seat.Interface
{
    public interface ISeatService
    {
        public Task<IEnumerable<SeatResponseDTO>> GetSeatsByBusIdAsync(int busId);
    }
}
