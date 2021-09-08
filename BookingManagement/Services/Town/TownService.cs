﻿using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Town;
using BookingManagement.Models.DTO.Town.Response;
using BookingManagement.Services.Town.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Town
{
    public class TownService : ITownService
    {
        private readonly BookingManagementDbContext _dbContext;
        private readonly ILogger<TownService> _logger;

        public TownService(BookingManagementDbContext dbContext,
            ILogger<TownService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<LoadMoreList<TownResponse>> GetTownsAsync(TownRequest request)
        {
            try
            {
                var result = _dbContext.Towns.AsNoTracking().Select(x => new TownResponse{ 
                    Country = x.Country,
                    Lat = x.Latitude,
                    Long = x.Longitude,
                    Name = x.Name
                });

                var listResult = LoadMoreList<TownResponse>.ToLoadMoreListAsync(result, request.CurrentPage, request.ItemsPerPage);

                return listResult;
            }catch(Exception ex)
            {
                _logger.LogError(ex, nameof(GetTownsAsync));
                throw;

            }
        }
    }
}
