using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BMC.Security.MobileApp.Pages
{
    public class TabMenu : TabbedPage
    {
        public TabMenu()
        {
            this.BarBackgroundColor = Color.FromHex("#3499EC");
            this.BarTextColor = Color.White;            
            Children.Add(new Home());
            Children.Add(new Contact());

        }
    }
}
