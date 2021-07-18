using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messaging;

namespace AuthService.Events
{
    public class UserRegistered : Event
    {
        public readonly string Id;
        public readonly string UserName;
        public readonly string Email;
        public readonly string ConfirmationLink; 

        public UserRegistered(Guid messageId, string userId, string userName, string email, string confirmationLink) : base(messageId)
        {
            Id = userId;
            UserName = userName;
            Email = email;
            ConfirmationLink = confirmationLink;
        }

    }
}
