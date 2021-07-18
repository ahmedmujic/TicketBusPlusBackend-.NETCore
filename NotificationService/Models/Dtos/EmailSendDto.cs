using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Models.Dtos
{
    public class EmailSendDto
    {
        public string To { get; set; }
        public string  Header { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        public EmailSendDto()
        {
            IsHtml = false;
        }
    }
}
