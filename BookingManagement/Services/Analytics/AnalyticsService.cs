using AutoMapper;
using BookingManagement.Models.DTO.Analytics.Response;
using BookingManagement.Models.DTO.Route.Response;
using BookingManagement.Services.Analytics.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly BookingManagementDbContext _dbContext;
        private readonly ILogger<AnalyticsService> _logger;
        private readonly IMapper _mapper;

        public AnalyticsService(BookingManagementDbContext dbContext,
            ILogger<AnalyticsService> logger,
             IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<GetRouteCountriesDTO>> GetCountriesAsync(string userId)
        {
            try
            {

                var result = await _dbContext.Routes.AsNoTracking()
                    .Include(r => r.EndStation)
                    .ThenInclude(r => r.Town)
                    .Where(r => r.CompandyId == userId )
                    .GroupBy(r => new {
                        Country = r.EndStation.Town.Country
                    })
                    .Select(r => new GetRouteCountriesDTO
                    {
                        CountryName = r.Key.Country,
                        Income = r.Sum(r => r.Price * r.SellCounter)
                    })
                    .OrderBy(r => r.Income)
                    .Take(5)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetCountriesAsync));
                throw;
            }
        }

        public async Task<List<MonthAnalyticsDTO>> GetMonthAnalyticsAsync(string userId)
        {
            try
            {

                var result = await _dbContext.Routes.AsNoTracking()
                    .Where(r => r.CompandyId == userId && r.StartingDate.Year == DateTime.Now.Year)
                    .GroupBy(r => new {
                        StartingDate = r.StartingDate.Month
                    })
                    .Select(r => new MonthAnalyticsDTO
                    {
                        Month = r.Key.StartingDate,
                        Income = r.Sum(r => r.Price * r.SellCounter)
                    })                    
                    .OrderBy(r => r.Month)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetMonthAnalyticsAsync));
                throw;
            }
        }

        public async Task<TimeStatDataResponseDTO> GetRouteStatsAsync(string userId)
        {
            try
            {

                var result = new TimeStatDataResponseDTO
                {
                    LastMonthIncome = await _dbContext.Routes.Where(r => r.StartingDate.Month == DateTime.Now.Month - 1).SumAsync(r => r.SellCounter * r.Price),
                    LastYearIncome = await _dbContext.Routes.Where(r => r.StartingDate.Year == DateTime.Now.Year - 1).SumAsync(r => r.SellCounter * r.Price),
                    Income = await _dbContext.Routes.SumAsync(r => r.SellCounter * r.Price)
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetRouteStatsAsync));
                throw;
            }
        }

    }
}
