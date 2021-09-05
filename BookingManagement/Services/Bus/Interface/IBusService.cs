using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Bus.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Bus.Interface
{
    public interface IBusService
    {
        public Task<AddBusResponseDTO> AddBusAsync(AddBusRequestDTO request);
        public Task<AddBusResponseDTO> AddBusFromCsvAsync(IFormFile file, string companyId);
        public Task<PaginationListResponse<BusResponseDTO>> GetBussesAsync(PaginationRequestDTO request);
    }
}
