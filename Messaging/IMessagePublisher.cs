using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messaging
{
    public interface IMessagePublisher
    {
        Task PublishMessageAsync(string messageType, object message, string routingKey); 
    }
}
