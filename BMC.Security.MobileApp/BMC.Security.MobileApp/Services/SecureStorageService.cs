using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BMC.Security.MobileApp.Services
{
    public static class SecureStorageService
    {
        public static Task SaveLogin(string username, string password) => SecureStorage.SetAsync(username, password);

        public static async Task<bool> IsLoginCorrect(string username, string password)
        {
            try
            {
                var savedPassword = await SecureStorage.GetAsync(username).ConfigureAwait(false);

                return password.Equals(savedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}
