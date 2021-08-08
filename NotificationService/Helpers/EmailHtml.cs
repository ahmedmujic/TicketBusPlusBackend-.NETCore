using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Helpers
{
    public static class EmailHtml
    {
        public static string EmailActivation(string FirstName, string activationLink)
        {
            return @"
                    <div><p>Hi, "+ FirstName + @"</p>
                        <p>To complete your registration, please verify your email:</p>
                        <a href='" + activationLink + @"' style='margin-top: 20px''></p>
                          Verify
                          </a>
                        <p> Thank you, The DiNero Team </p>";
        }

        public static string ResetPassword(string FirstName, string link)
        {
            return @"
                    <div><p>Hi, " + FirstName + @"</p>
                        <p>To reset your password click the next link:</p>
                        <a href='" + link + @"' style='margin-top: 20px''></p>
                          Reset
                          </a>
                        <p> Thank you, The BusPlus Team </p>";
        }
    }
}
