using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Route;
using BookingManagement.Models.DTO.Route.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Town.Interfaces
{
    public interface IRouteService
    {
        Task<AddRouteResponseDTO> AddRouteAsync(AddRouteRequestDTO request);
        Task<IEnumerable<TopSellingResponseDTO>> GetTopSellingRoutesAsync(string userId);
        Task<PaginationListResponse<RouteResponseDTO>> GetRoutesAsync(PaginationRequestDTO request);
    }
}
