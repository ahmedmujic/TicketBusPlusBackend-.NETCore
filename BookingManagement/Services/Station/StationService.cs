using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Station.Response;
using BookingManagement.Services.Station.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Station
{
    public class StationService : IStationService
    {
        private readonly BookingManagementDbContext _dbContext;
        private readonly ILogger<StationService> _logger;

        public StationService(BookingManagementDbContext dbContext,
            ILogger<StationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PaginationListResponse<StationResponseDTO>> GetStationsAsync(PaginationRequestDTO request)
        {
            try
            {
                var result = _dbContext.Station.AsNoTracking()
                    .Include(s => s.Town)
                    .Select(s => new StationResponseDTO
                    {
                        City = s.Town.Name,
                        Name = s.Name,
                        StationId = s.Id
                    });

                var paginatedResult = await PaginationList<StationResponseDTO>.ToPaginationListAsync(result, request.CurrentPage, request.ItemsPerPage).ConfigureAwait(false);

                return new PaginationListResponse<StationResponseDTO>
                {
                    CurrentPage = paginatedResult.CurrentPage,
                    ItemsCount = paginatedResult.ItemsCount,
                    ItemsPerPage = paginatedResult.ItemsPerPage,
                    PageOffset = paginatedResult.PageOffset,
                    Data = paginatedResult
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetStationsAsync));
                throw;
            }
        }
    }
}
