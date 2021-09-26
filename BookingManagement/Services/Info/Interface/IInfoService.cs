using BookingManagement.Models.DTO.Info.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Info.Interface
{
    public interface IInfoService
    {
        Task<bool> SendInfoEmailAsync(SendInfoMailDTO request);
    }
}
