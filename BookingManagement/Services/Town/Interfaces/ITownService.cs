using BookingManagement.Helpers;
using BookingManagement.Models.DTO.Town;
using BookingManagement.Models.DTO.Town.Response;
using BookingManagement.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Town.Interfaces
{
    public interface ITownService
    {
        public Task<LoadMoreList<TownResponse>> GetTownsAsync(TownRequest request);
    }
}
