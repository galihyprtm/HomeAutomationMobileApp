using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BMC.Security.MobileApp.Views.Login
{
    public class StyledLabel : Label
    {
        public StyledLabel()
        {
            TextColor = Color.White;
            FontFamily = App.GetDefaultFontFamily();
            FontSize = 14;
        }
    }
}
