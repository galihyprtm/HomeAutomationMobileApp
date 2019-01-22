using BMC.Security.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BMC.Security.Web
{
    public partial class _Default : Page
    {
        static AzureIoT iot = new AzureIoT();
        protected void Page_Load(object sender, EventArgs e)
        {
            BtnPass.Click += BtnPass_Click;
            BtnMonster.Click += DoAction;
            BtnTornado.Click += DoAction;
            BtnPolice.Click += DoAction;
            BtnScream.Click += DoAction;
            BtnLedOff.Click += DoAction;
            BtnLedOn.Click += DoAction;
            BtnCCTVOn.Click+= DoAction;
            BtnCCTVOff.Click+= DoAction;
            BtnCCTVInterval.Click+= DoAction;
            BtnRelay1.Click += DoAction;
            BtnRelay1Off.Click += DoAction;
            BtnRelay2.Click += DoAction;
            BtnRelay2Off.Click += DoAction;
            WaterInBtn1.Click += DoAction;
            WaterInBtn2.Click += DoAction;
            WaterOutBtn1.Click += DoAction;
            WaterOutBtn2.Click += DoAction;
            if (!IsPostBack)
            {
                var data = DeviceData.GetAllDevices();
                RptControlDevice.DataSource = data;
                RptControlDevice.DataBind();
                           
            }
        }

        protected async void DoAction(object sender, EventArgs e)
        {
            try
            {
                TxtStatus.Text = "";
                var btn = sender as Button;
                var tipe = btn.CommandName;
                switch (tipe)
                {
                    case "Monster":
                        await iot.InvokeMethod("BMCSecurityBot","PlaySound", new string[] { "monster.mp3" });
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
                            string URL = $"http://{btn.CommandArgument}/cm?cmnd=Power%20On";
                            await iot.InvokeMethod("BMCSecurityBot", "OpenURL", new string[] { URL });
                        }
                        break;
                    case "DEVICEOFF":
                        {
                            //string DeviceID = $"Device{btn.CommandArgument}IP";
                            string URL = $"http://{btn.CommandArgument}/cm?cmnd=Power%20Off";
                            await iot.InvokeMethod("BMCSecurityBot", "OpenURL", new string[] { URL });
                        }
                        break;
                    case "CCTVStatus":
                        await iot.InvokeMethod("CCTV_Watcher", "CCTVStatus", new string[] { btn.CommandArgument });
                        break;
                    case "CCTVUpdateTime":
                        var interval = string.IsNullOrEmpty(TxtInterval.Text) ? "10" : TxtInterval.Text;
                        await iot.InvokeMethod("CCTV_Watcher", "CCTVUpdateTime", new string[] { interval });

                        break;
                    case "Relay1":
                        await iot.InvokeMethod("hidroponik", "Relay1", new string[] { btn.CommandArgument });
                        break;
                    case "Relay2":
                        await iot.InvokeMethod("hidroponik", "Relay2", new string[] { btn.CommandArgument });
                        break;
                    case "WaterIn":
                        await iot.InvokeMethod("aquascape", "WaterIn", new string[] { btn.CommandArgument });
                        break;
                    case "WaterOut":
                        await iot.InvokeMethod("aquascape", "WaterOut", new string[] { btn.CommandArgument });
                        break;
                }
            }
            catch(Exception ex)
            {
                TxtStatus.Text = "ERROR:"+ex.Message + "_" + ex.StackTrace;
            }
        }

        private void BtnPass_Click(object sender, EventArgs e)
        {
          if(TxtPass.Text == "123qweasd")
            {
                ControlPanel.Visible = true;
                PassPanel.Visible = false;
            }
            else
            {
                TxtLogin.Text = "Passcode salah goblok!!";
            }
        }
    }
}