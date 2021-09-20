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
                    .Where(r => r.CompandyId == userId)
                    .Take(10)
                    .Select(r => new GetRouteCountriesDTO
                    {
                        CountryName = r.EndStation.Town.Country,
                        Income = _dbContext.Routes.AsNoTracking().Where(t => t.EndStation.Town.Country == r.EndStation.Town.Country).Sum(r => r.SellCounter * r.Price)
                    })
                    .Distinct()
                    .ToListAsync();


                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetCountriesAsync));
                throw;
            }
        }

        public async Task<List<MonthAnalyticsDTO>> GetMonthAnalytics(string userId)
        {
            try
            {

                var result = await _dbContext.Routes.AsNoTracking()
                    .Where(r => r.CompandyId == userId)
                    .GroupBy(r => new {
                        StartingDate = r.StartingDate,
                        Income = r.Price * r.SellCounter
                    })
                    .Select(r => new MonthAnalyticsDTO
                    {
                        Date = r.Key.StartingDate,
                        Income = r.Key.Income
                    })
                    .OrderBy(r => r.Date)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetMonthAnalytics));
                throw;
            }
        }

        public async Task<List<TimeStatDataResponseDTO>> GetRouteStatsAsync(string userId)
        {
            try
            {

                var result = await _dbContext.Routes.AsNoTracking()
                    .Where(r => r.CompandyId == userId)
                    .Select(_ => new TimeStatDataResponseDTO
                    {
                        LastMonthIncome = _dbContext.Routes.Where(r => r.StartingDate.Month == DateTime.Now.Month - 1).Select(r => r.SellCounter * r.Price).First(),
                        LastYearIncome = _dbContext.Routes.Where(r => r.StartingDate.Year == DateTime.Now.Year - 1).Select(r => r.SellCounter * r.Price).First(),
                        Income = _dbContext.Routes.Select(r => r.SellCounter * r.Price).First(),
                    }).ToListAsync();


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
