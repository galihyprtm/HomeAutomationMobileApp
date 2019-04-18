using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using BMC.Security.MobileApp;
using BMC.Security.MobileApp.Droid;

using Plugin.CurrentActivity;
using Xamarin.Forms;
using Android.Graphics;
using BMC.Security.MobileApp.Views.Base;
using BMC.Security.MobileApp.Droid.CustomRenders;

[assembly: ExportRenderer(typeof(FontAwesomeIcon), typeof(FontAwesomeIconRenderer))]
namespace BMC.Security.MobileApp.Droid.CustomRenders
{
    public class FontAwesomeIconRenderer: LabelRenderer
    {
        #region Constructors
        public FontAwesomeIconRenderer(Context context) : base(context)
        {
        }
        #endregion

        #region Properties
        Context CurrentContext => CrossCurrentActivity.Current.Activity;
        #endregion

        #region Methods
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement is null)
                Control.Typeface = Typeface.CreateFromAsset(CurrentContext.Assets, "FontAwesome.ttf");
        }
        #endregion
    }
}