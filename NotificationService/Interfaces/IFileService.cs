using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Interfaces
{
    public interface IFileService
    {
        byte[] CreateInvoicePDF(string body);
    }
}
