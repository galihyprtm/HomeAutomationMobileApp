using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BMC.Security.MobileApp.Views.Login
{
    public class StyledEntry : Entry
    {
        public StyledEntry(double opacity = 0)
        {
            BackgroundColor = Color.Transparent;
            HeightRequest = 40;
            TextColor = Color.White;
            Opacity = opacity;
            PlaceholderColor = Color.White;
        }
    }
}
