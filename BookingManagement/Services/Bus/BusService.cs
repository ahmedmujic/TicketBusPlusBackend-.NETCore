using AutoMapper;
using BookingManagement.Helpers;
using BookingManagement.Models.Bulk;
using BookingManagement.Models.Csv;
using BookingManagement.Models.Domain;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Bus.Response;
using BookingManagement.Services.Bus.Interface;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Bus
{
    public class BusService : IBusService
    {
        private readonly BookingManagementDbContext _dbContext;
        private readonly ILogger<BusService> _logger;
        private readonly IMapper _mapper;

        public BusService(BookingManagementDbContext dbContext,
           ILogger<BusService> logger,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AddBusResponseDTO> AddBusAsync(AddBusRequestDTO request)
        {
            try
            {
               
                var busModel = new Models.Domain.Bus()
                {
                    Name = request.Name,
                    NumberOfSeats = request.NumberOfSeats,
                    CompanyId = request.UserId
                };

                List<Seat> seats = new();
                for (var i = 0; i < request.NumberOfSeats; i++)
                {
                    seats.Add(new Seat
                    {
                        Checked = false,
                        SeatCode = Guid.NewGuid().ToString(),
                        Bus = busModel
                    });
                }

                await _dbContext.AddAsync(busModel);
                await _dbContext.AddRangeAsync(seats);
                await _dbContext.SaveChangesAsync();

                return new AddBusResponseDTO();

            }catch(Exception ex)
            {
                _logger.LogError(ex, nameof(AddBusAsync));
                throw;
            }
        }

        public async Task<AddBusResponseDTO> AddBusFromCsvAsync(IFormFile file, string companyId)
        {
            try
            {
               /*List<Models.Domain.Bus> busses = null;
                using (var fileStream = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(fileStream, CultureInfo.InvariantCulture))
                {
                    busses = new List<Models.Domain.Bus>();
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = new Models.Domain.Bus
                        {
                            Name = csv.GetField<string>("Name"),
                            NumberOfSeats = csv.GetField<int>("NumberOfSeats"),
                            CompanyId = companyId,
                            BusPicture = null
                            
                        };
                        busses.Add(record);
                    }

                }

                await _dbContext.Buses.AddRangeAsync()

                BulkInserBusses config = new(busses, companyId)
                {
                    BatchSize = 5000,
                    ConnectionString = _dbContext.Database.GetConnectionString(),
                    DestinationTable = "Buses"
                };

                await BulkInsert.BulkInsertAsync(config).ConfigureAwait(false);
                */

                return new AddBusResponseDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(AddBusFromCsvAsync));
                throw;
            }
        }

        public async Task<PaginationListResponse<BusResponseDTO>> GetBussesAsync(PaginationRequestDTO request)
        {
            try
            {
                var result = _dbContext.Buses.AsNoTracking()
                     .Where(r => r.CompanyId == request.UserId)
                        .Select(x => new BusResponseDTO
                        {
                            Id = x.Id,
                            Name = x.Name,
                            NumberOfSeats = x.NumberOfSeats,
                            Pic = x.BusPicture
                        });


                var paginatedResult = await PaginationList<BusResponseDTO>.ToPaginationListAsync(result, request.CurrentPage, request.ItemsPerPage).ConfigureAwait(false);

                return new PaginationListResponse<BusResponseDTO> {
                    CurrentPage = paginatedResult.CurrentPage,
                    ItemsCount = paginatedResult.ItemsCount,
                    ItemsPerPage = paginatedResult.ItemsPerPage,
                    PageOffset = paginatedResult.PageOffset,
                    Data = paginatedResult
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetBussesAsync));
                throw;
            }
        }
    }
}
