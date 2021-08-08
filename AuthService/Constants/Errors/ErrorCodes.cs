using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Constants.Errors
{
    public class ErrorCodes
    {
        public const string AccountNotConfirmed = "AccountNotConfirmed";
        public const string AccountAlreadyConfirmed = "AccountAlreadyConfirmed";
        public const string InvalidFormat = "InvalidFormat";
        public const string UserNotFound = "UserNotFound";
    }
}
