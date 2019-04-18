using System;
using System.Collections.Generic;
using System.Text;

namespace BMC.Security.MobileApp.CustomEventArgs
{
    public class LoginFailedEventArgs
    {
        public LoginFailedEventArgs(string errorMessage, bool shouldDisplaySignUpPrompt)
        {
            ErrorMessage = errorMessage;
            ShouldDisplaySignUpPrompt = shouldDisplaySignUpPrompt;
        }

        public string ErrorMessage { get; }
        public bool ShouldDisplaySignUpPrompt { get; }
    }
}
