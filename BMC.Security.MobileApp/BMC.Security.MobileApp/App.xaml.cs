using BMC.Security.MobileApp.PagesLogin;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BMC.Security.MobileApp
{
    public partial class App : Application
    {
        //public App()
        //{
        //    InitializeComponent();

        //    MainPage = new PagesLogin.LoginPage();
        //}
        public App() => MainPage = new ContentPage();

        public static void InitializeMainPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var loginPage = new LoginPage();

                Current.MainPage = new NavigationPage(loginPage)
                {
                    BarBackgroundColor = Color.FromHex("#3498db"),
                    BarTextColor = Color.White,
                };

                NavigationPage.SetHasNavigationBar(loginPage, false);
            });
        }

        public static string GetDefaultFontFamily()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return "AppleSDGothicNeo-Light";
                case Device.Android:
                    return "Droid Sans Mono";
                default:
                    throw new NotSupportedException("Platform Not Supported");
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            base.OnStart();

            InitializeMainPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
