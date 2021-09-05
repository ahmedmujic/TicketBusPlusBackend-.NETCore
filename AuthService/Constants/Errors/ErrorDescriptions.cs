using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Constants.Errors
{
    public class ErrorDescriptions
    {
        public const string AccountNotConfirmed = "Account is not confirmed";
        public const string TokenUserRequired = "UserId and token are required";
        public const string UserNotFoundEmail = "User does not exist";
        public const string AccountAlreadyConfirmed = "Account already confirmed";

    }
}
