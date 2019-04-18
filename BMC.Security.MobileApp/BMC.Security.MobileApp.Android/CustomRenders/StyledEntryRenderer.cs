using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BMC.Security.MobileApp.Droid.CustomRenders;
using BMC.Security.MobileApp.Views.Login;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(StyledEntry), typeof(StyledEntryRenderer))]
namespace BMC.Security.MobileApp.Droid.CustomRenders
{
    public class StyledEntryRenderer : EntryRenderer
    {
        public StyledEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.SetHintTextColor(Xamarin.Forms.Color.White.ToAndroid());
                Control.Typeface = Typeface.Create(App.GetDefaultFontFamily(), TypefaceStyle.Normal);
            }
        }
    }
}