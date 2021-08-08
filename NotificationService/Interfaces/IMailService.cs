using NotificationService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(UserRegistered to, string subject, string body, bool isHtml, string fullName);
    }
}