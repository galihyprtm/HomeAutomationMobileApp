using BMC.Security.MobileApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BMC.Security.MobileApp.PagesLogin
{
    public abstract class BaseContentPage<T> : ContentPage where T: BaseViewModel, new()
    {
        #region Constructors
        protected BaseContentPage() => BindingContext = ViewModel;
        #endregion

        #region Properties
        protected T ViewModel { get; } = new T();
        #endregion
    }
}
