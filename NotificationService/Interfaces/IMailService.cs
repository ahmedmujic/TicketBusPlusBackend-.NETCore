using MimeKit;
using NotificationService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(string emailBody, string reciever, string subject);
        Task<bool> SendEmailAsync(BodyBuilder body, string reciever, string subject);
    }
}