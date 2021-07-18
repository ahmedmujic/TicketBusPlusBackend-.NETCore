using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Events
{
    public class ResetPassword : Event
    {
        public readonly string Id;
        public readonly string FirstName;
        public readonly string Email;
        public readonly string ConfrimationLink;
        public ResetPassword(Guid messageId, string userId, string firstName, string email, string confirmationLink) : base(messageId)
        {
            Id = userId;
            FirstName = firstName;
            Email = email;
            ConfrimationLink = confirmationLink;
        }
    }
}
