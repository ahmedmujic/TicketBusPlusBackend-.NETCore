using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Helpers
{
    public static class EmailHtml
    {
        public static string EmailActivation(string activationLink)
        {
            return @"
                    <div><p>Hi, </p>
                        <p>To complete your registration, please verify your email:</p>
                        <a href=href='" + activationLink + @"' style='margin-top: 20px''></p>
                          Verify
                          </a>
                        <p> Thank you, The DiNero Team </p>";
        }
    }
}
