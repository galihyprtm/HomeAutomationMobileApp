using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BMC.Security.MobileApp.Services
{
    public static class DebugService
    {
        #region Methods
        [Conditional("DEBUG")]
        public static void PrintException(
            Exception exception,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerFilePath] string filePath = "")
        {
            var fileName = System.IO.Path.GetFileName(filePath);

            Debug.WriteLine(exception.GetType());
            Debug.WriteLine($"Error: {exception.Message}");
            Debug.WriteLine($"Line Number: {lineNumber}");
            Debug.WriteLine($"Caller Name: {callerMemberName}");
            Debug.WriteLine($"File Name: {fileName}");
        }
        #endregion
    }
}
