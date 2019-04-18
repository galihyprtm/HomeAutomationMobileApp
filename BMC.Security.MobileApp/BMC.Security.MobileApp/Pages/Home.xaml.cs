using BMC.Security.MobileApp.Helpers;
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
	public partial class Home : ContentPage
	{
        public static MqttService iot;
        public Home()
        {
            InitializeComponent();
            try
            {
                //iot = new MqttService();
                BtnMonster.Clicked += DoAction;
                BtnScream.Clicked += DoAction;
                BtnTornado.Clicked += DoAction;
                BtnPolice.Clicked += DoAction;
                btnOff.Clicked += Wcoff;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        protected async void DoAction(object sender, EventArgs e)
        {
            try
            {
                //iot = new MqttService();
                var btn = sender as Button;
                var tipe = btn.CommandParameter;
                switch (tipe)
                {
                    case "Monster":
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "monster.mp3" });
                        break;
                    case "Scream":
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "scream.mp3" });
                        break;
                    case "Tornado":
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "tornado.mp3" });
                        break;
                    case "Police":
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "police.mp3" });
                        break;
                    case "LEDON":
                        await iot.InvokeMethod("BMCSecurityBot", "ChangeLED", new string[] { "RED" });
                        break;
                    case "LEDOFF":
                        await iot.InvokeMethod("BMCSecurityBot", "TurnOffLED", new string[] { "" });
                        break;
                    case "DEVICEON":
                        {
                            //string DeviceID = $"Device{btn.CommandArgument}IP";
                            string URL = $"http://{btn.CommandParameter}/cm?cmnd=Power%20On";
                            await iot.InvokeMethod("BMCSecurityBot", "OpenURL", new string[] { URL });
                        }
                        break;
                    case "DEVICEOFF":
                        {
                            //string DeviceID = $"Device{btn.CommandArgument}IP";
                            string URL = $"http://{btn.CommandParameter}/cm?cmnd=Power%20Off";
                            await iot.InvokeMethod("BMCSecurityBot", "OpenURL", new string[] { URL });
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
        protected async void Wcoff(object sender, EventArgs e)
        {
            var btn = sender as Button;
            try
            {
                string URL = $"http://{btn.CommandParameter}/cm?cmnd=Power%20Off";
                await iot.InvokeMethod("BMCSecurityBot", "OpenURL", new string[] { URL });
            }
            catch (Exception er)
            {
                await DisplayAlert("Error", er.ToString(), "OK");
            }
        }
    }
}