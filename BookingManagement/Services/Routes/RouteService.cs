using AutoMapper;
using BookingManagement.Helpers;
using BookingManagement.Models.Domain;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Route;
using BookingManagement.Models.DTO.Route.Response;
using BookingManagement.Services.Town.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routes = BookingManagement.Models.Domain.Routes;




namespace BookingManagement.Services.Routes
{
    public class RouteService : IRouteService
    {
        private readonly BookingManagementDbContext _dbContext;
        private readonly ILogger<RouteService> _logger;
        private readonly IMapper _mapper;

        public RouteService(BookingManagementDbContext dbContext,
            ILogger<RouteService> logger,
             IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AddRouteResponseDTO> AddRouteAsync(AddRouteRequestDTO request)
        {
            try
            {
                List<Dates> dates = new();

                foreach (var date in request.Dates)
                    dates.Add(new Dates
                    {
                        Date = date
                    });

                await _dbContext.Dates.AddRangeAsync(dates);

                var routeModel = new Models.Domain.Routes()
                {
                    Price = request.Price,
                    BusId = request.BusId,
                    StartStationId = request.StartStationId,
                    EndStationId = request.EndStationId,
                    Dates = dates,
                    CompandyId = request.CompanyId
                };               

                await _dbContext.Routes.AddAsync(routeModel);
                await _dbContext.SaveChangesAsync();

                return new AddRouteResponseDTO();
            }catch(Exception ex)
            {
                _logger.LogError(ex, nameof(AddRouteAsync));
                throw;
            }
        }

        public async Task<PaginationListResponse<RouteResponseDTO>> GetRoutesAsync(PaginationRequestDTO request)
        {
            try
            {
                var result = _dbContext.Routes.AsNoTracking()
                    .Include(r => r.Bus)
                    .Include(r => r.Dates)
                    .Include(r => r.EndStation)
                    .ThenInclude(s => s.Town)
                    .Include(r => r.StartStation)
                    .ThenInclude(s => s.Town)
                    .Where(r => r.CompandyId == request.UserId)
                    .Select(r => new RouteResponseDTO
                    {
                        BusName = r.Bus.Name,
                        Dates = r.Dates.Select(d => d.Date),
                        Price = r.Price,
                        EndingStation = $"{r.EndStation.Name}, {r.EndStation.Town.Name}",
                        StartingStation = $"{r.StartStation.Name}, {r.StartStation.Town.Name}",
                        Sells = r.SellCounter
                    });

                var paginatedResult = await PaginationList<RouteResponseDTO>.ToPaginationListAsync(result, request.CurrentPage, request.ItemsPerPage).ConfigureAwait(false);

                return new PaginationListResponse<RouteResponseDTO>
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
                _logger.LogError(ex, nameof(GetRoutesAsync));
                throw;
            }
        }

        public async Task<IEnumerable<TopSellingResponseDTO>> GetTopSellingRoutesAsync(string companyId)
        {
            try
            {
                var result = _dbContext.Routes.AsNoTracking()
                    .Include(s => s.StartStation.Town).Include(e => e.EndStation.Town);

                var sum = Convert.ToDecimal(result.SumAsync(x => x.SellCounter));


                var result2 = await result.Select(x => new TopSellingResponseDTO
                    {
                        RouteId = x.Id,
                        CompanyId = x.CompandyId,
                        RouteName = $"{x.StartStation.Town.Name} - {x.EndStation.Town.Name}",
                        Percentage = Decimal.Round(((x.SellCounter * 100) / sum ), 1)
                    })
                    .Where(r => r.CompanyId == companyId)
                    .OrderByDescending(x => x.Percentage).Take(5).ToListAsync();              


                return result2;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetTopSellingRoutesAsync));
                throw;
            }
        }
    }
}
