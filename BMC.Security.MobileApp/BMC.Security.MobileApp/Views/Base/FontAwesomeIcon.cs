using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Drawing;

namespace BMC.Security.MobileApp.Views.Base
{
    public class FontAwesomeIcon : Label
    {
        public const char CheckedBox = '\uf046', EmptyBox = '\uf096';
        const string _typeface = "FontAwesome";

        public FontAwesomeIcon() => FontFamily = _typeface;
    }
}
