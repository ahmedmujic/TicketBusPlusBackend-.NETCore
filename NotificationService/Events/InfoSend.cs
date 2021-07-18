using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Events
{
    public class InfoSend : UserRegistered
    {
        public readonly string FullName;
        public readonly string PhoneNumber;
        public readonly string Message;

        public InfoSend(Guid messageId, string fullName, string email, string phoneNumber, string message) : base(messageId, null, fullName, email, null)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Message = message;
        }
    }
}
