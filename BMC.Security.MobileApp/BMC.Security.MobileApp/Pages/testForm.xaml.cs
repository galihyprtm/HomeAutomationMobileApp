using BMC.Security.MobileApp.Helpers;
using BMC.Security.MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMC.Security.MobileApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class testForm : ContentPage
	{       
		public testForm ()
		{
            InitializeComponent();
        }
        private void ListViewItem_Tabbed(object sender, ItemTappedEventArgs e)
        {
            //var product = e.Item as Product;
            //var vm = BindingContext as MainList;
            //vm?.ShoworHiddenProducts(product);
        }
    }
}