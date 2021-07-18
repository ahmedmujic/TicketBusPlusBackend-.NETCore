using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Events
{
    public class InvoiceSend : UserRegistered
    {
        public readonly string Amount;
        public readonly string Description;
        public readonly List<string> SeatNumbers;

        public InvoiceSend(Guid messageId, decimal amount, string description, string email, List<string> seatNumbers, string firstName) : base(messageId, null, firstName, email, null)
        {
            Description = description;
            SeatNumbers = seatNumbers;
            Amount = amount.ToString();
        }
    }
}
