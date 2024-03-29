﻿using System;
using System.Collections.Generic;
using System.Text;
using BMC.Security.MobileApp.Services;
using BMC.Security.MobileApp.ViewModel;
using Plugin.Permissions;
using Xamarin.Forms;

namespace BMC.Security.MobileApp.PagesLogin
{
    public abstract class BaseMediaContentPage<T> : BaseContentPage<T> where T : BaseViewModel, new()
    {
        protected BaseMediaContentPage()
        {
            PhotoService.PermissionsDenied += HandlePermissionsDenied;
            PhotoService.NoCameraDetected += HandleNoPhotoDetected;
        }

        void HandlePermissionsDenied(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var isAlertAccepted = await DisplayAlert("Open Settings?", "Storage and Camera Permission Need To Be Enabled", "Ok", "Cancel");
                if (isAlertAccepted)
                    CrossPermissions.Current.OpenAppSettings();
            });
        }

        void HandleNoPhotoDetected(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error", "Camera Not Available", "OK"));
    }
}
