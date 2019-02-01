using BMC.Security.Models;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GIS = GHIElectronics.UWP.Shields;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;

namespace BMC_Security.Guard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       

        private GIS.FEZHAT hat;
        private DispatcherTimer timer;
        bool IsConnected = false;
        static HttpClient client;
       
        public MainPage()
        {
            this.InitializeComponent();

            Setup();
          
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(10 * 60 * 1000); //10 minutes
            this.timer.Tick += this.OnTick;
            this.timer.Start();
        }

        /// <summary>
        /// Play sound from assets
        /// </summary>
        /// <param name="SoundFile">name of wav file: audio.wav</param>
        async void PlaySound(string SoundFile)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {

                MediaElement mysong = new MediaElement();
                Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
                Windows.Storage.StorageFile file = await folder.GetFileAsync(SoundFile);
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                mysong.SetSource(stream, file.ContentType);
                mysong.Play();
                //UI code here
            });
        }

        void ChangeLED(GIS.FEZHAT.Color SelColor)
        {
            this.hat.D2.Color = SelColor;
            this.hat.D3.Color = SelColor;
        }

        async void Setup()
        {
            try
            {
                if (!IsConnected)
                {
                    if (s_deviceClient != null)
                    {
                        s_deviceClient.Dispose();
                    }
                    // Connect to the IoT hub using the MQTT protocol
                    s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Mqtt);
                    s_deviceClient.SetMethodHandlerAsync("DoAction", DoAction, null).Wait();
                    //SendDeviceToCloudMessagesAsync();
                    BtnPlay.Click += (a, b) => { PlaySound("monster.mp3"); };

                    this.hat = await GIS.FEZHAT.CreateAsync();

                    this.hat.S1.SetLimits(500, 2400, 0, 180);
                    this.hat.S2.SetLimits(500, 2400, 0, 180);


                    IsConnected = true;
                }
                if (client == null)
                {
                    client = new HttpClient();
                }
            }
            catch
            {

            }

           
        }
        private async void OnTick(object sender, object e)
        {
            try
            {
                if (IsConnected)
                {
                    double x, y, z;

                    this.hat.GetAcceleration(out x, out y, out z);
                    var light = this.hat.GetLightLevel();
                    var temp = this.hat.GetTemperature();
                    var item = new EnvData() { Accel = (x, y, z), Light = light, Temp = temp, LocalTime = DateTime.Now };
                    SendDeviceToCloudMessagesAsync(item);
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        TxtLight.Text = light + " lux";
                        TxtTemp.Text = temp + " C";
                        TxtAccel.Text = x + "," + y + "," + z;
                        TxtTimeUpdate.Text = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    });
                }
                else
                {
                    Setup();
                }
            }
            catch
            {
                IsConnected = false;
            }
        }

            // Handle the direct method call
            private async Task<MethodResponse> DoAction(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            var action = JsonConvert.DeserializeObject<DeviceAction>(data);
            // Check the payload is a single integer value
            if (action!=null)
            {
                /*
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Telemetry interval set to {0} seconds", data);
                Console.ResetColor();
                */
                switch (action.ActionName)
                {
                    case "PlaySound":
                        PlaySound(action.Params[0]);
                        break;
                    case "ChangeLED":
                        ChangeLED(GIS.FEZHAT.Color.Red);
                        break;
                    case "TurnOffLED":
                        ChangeLED(GIS.FEZHAT.Color.Black);
                        break;
                    case "OpenURL":
                        var res = await OpenUrl(action.Params[0]);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            TxtStatus.Text = $"open : {action.Params[0]} => {res}";
                        });
                        break;
                   
                }
                // Acknowlege the direct method call with a 200 success message
                string result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\"}";
                return new MethodResponse(Encoding.UTF8.GetBytes(result), 200);
            }
            else
            {
                // Acknowlege the direct method call with a 400 error message
                string result = "{\"result\":\"Invalid parameter\"}";
                return new MethodResponse(Encoding.UTF8.GetBytes(result), 400);
            }
        }

        async Task<bool> OpenUrl(string URL)
        {
            try
            {
                var res = await client.GetAsync(URL);
                return res.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    TxtStatus.Text = ex.Message + "_" + ex.StackTrace;
                });
                return false;
            }
        }

        private static DeviceClient s_deviceClient;

        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private readonly static string s_connectionString = "HostName=FreeDeviceHub.azure-devices.net;DeviceId=BMCSecurityBot;SharedAccessKey=bjwkcj0aJc9BBoAhHBN6nidx/s7VODUt90rQBP4GaXE=";

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync(EnvData data)
        {
            var message = new Message(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

            // Add a custom application property to the message.
            // An IoT hub can filter on these properties without access to the message body.
            message.Properties.Add("temperatureAlert", (data.Temp > 40) ? "true" : "false");

            // Send the telemetry message
            await s_deviceClient.SendEventAsync(message);
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, "ok");
            
        }

        static async Task ReceiveCommands(DeviceClient deviceClient)
        {
            Console.WriteLine("\nDevice waiting for commands from IoTHub...\n");
            Message receivedMessage;
            string messageData;

            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync(TimeSpan.FromSeconds(1));

                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("\t{0}> Received message: {1}", DateTime.Now.ToLocalTime(), messageData);

                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
        }

    }

    
}
