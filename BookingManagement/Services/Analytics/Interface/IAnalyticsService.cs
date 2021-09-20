using BookingManagement.Models.DTO.Analytics.Response;
using BookingManagement.Models.DTO.Route.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Analytics.Interface
{
    public interface IAnalyticsService
    {

        Task<List<TimeStatDataResponseDTO>> GetRouteStatsAsync(string userId);
        Task<List<GetRouteCountriesDTO>> GetCountriesAsync(string userId);
        Task<List<MonthAnalyticsDTO>> GetMonthAnalytics(string userId);
    }
}
