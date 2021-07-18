using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Station.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Station.Interface
{
    public interface IStationService
    {
        Task<PaginationListResponse<StationResponseDTO>> GetStationsAsync(PaginationRequestDTO request);
    }
}
