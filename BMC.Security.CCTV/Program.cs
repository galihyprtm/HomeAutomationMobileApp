using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
//using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace BMC.Security.CCTV
{
    class Program
    {
        public static IConfigurationRoot Configuration;
        static AzureBlobHelper blobHelper;
        static AzureTableHelper tableHelper;
        static bool IsConnected = false;
        static bool IsPatrol = false;
        static int TimeInterval = 10;
        static string[] keywords = new  string[]{ "person","people","man","women","girl","boy"};
        // subscriptionKey = "0123456789abcdef0123456789ABCDEF"
        private const string subscriptionKey = "ed9496df54b54bca9a46ad451065b604";
        static string CCTV_IP = "";
        static int cctvCount = 4;
        static string[] cctvUrl = null;
        static string cctvpath = @"C:\Users\mifma\Documents\temp\";
        // localImagePath = @"C:\Documents\LocalImage.jpg"
        private const string localImagePath = @"<LocalImage>";

        private const string remoteImageUrl =
            "http://upload.wikimedia.org/wikipedia/commons/3/3c/Shaki_waterfall.jpg";

        // Specify the features to return
        private static readonly List<VisualFeatureTypes> features =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags, 
        };
        //private static DeviceClient s_deviceClient;
        static HttpClient client;
        static ComputerVisionClient computerVision;
        static void Main(string[] args)
        {
            if (client == null) client = new HttpClient();
            computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });
            computerVision.Endpoint = "https://southeastasia.api.cognitive.microsoft.com";
            Setup();
            Watcher();
            Console.ReadLine();
        }
        static MqttClient MqttClient;
        const string DataTopic = "bmc/homeautomation/data";
        const string ControlTopic = "bmc/homeautomation/control";

        static public void PublishMessage(string Message)
        {
            MqttClient.Publish(DataTopic, Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        static void SetupMqtt()
        {
            string IPBrokerAddress = "110.35.82.86"; //ConfigurationManager.AppSettings["MqttHost"];
            string ClientUser = "loradev_mqtt"; //ConfigurationManager.AppSettings["MqttUser"];
            string ClientPass = "test123";//ConfigurationManager.AppSettings["MqttPass"];

            MqttClient = new MqttClient(IPBrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            MqttClient.Subscribe(new string[] { ControlTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            MqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            var clientId = "bmc-gateway-2";//Guid.NewGuid().ToString();

            MqttClient.Connect(clientId, ClientUser, ClientPass);
            Console.WriteLine("MQTT is connected");
        } // this code runs when a message was received
        static async void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            if (e.Topic == ControlTopic)
            {
              await DoAction(ReceivedMessage);

            }
        }
        static void Setup()
        {
            try
            {
                
                // Set up configuration sources.
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                    .AddJsonFile("appsettings.json", optional: true);
                builder.AddJsonFile(
                           Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.Development.json"),
                           optional: true
                       );
               
                //add env vars
                //builder.AddEnvironmentVariables();
                //get config
                Configuration = builder.Build();

                CCTV_IP = Configuration.GetSection("server").GetSection("cctv-dvr-ip").Value;
                cctvUrl = new string[] { $"http://{CCTV_IP}/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=0&d=1&rand=",
        $"http://{CCTV_IP}/cgi-bin/snapshot.cgi?chn=1&u=admin&p=&q=0&d=1&rand=",
        $"http://{CCTV_IP}/cgi-bin/snapshot.cgi?chn=2&u=admin&p=&q=0&d=1&rand=",
        $"http://{CCTV_IP}/cgi-bin/snapshot.cgi?chn=3&u=admin&p=&q=0&d=1&rand="};

                if (!IsConnected)
                {
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\CCTV\\Gambar\\";
                    cctvpath = path;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    tableHelper = new AzureTableHelper("cctv");
                    blobHelper = new AzureBlobHelper();
                    /*
                    if (s_deviceClient != null)
                    {
                        s_deviceClient.Dispose();
                    }
                    // Connect to the IoT hub using the MQTT protocol
                    s_deviceClient = DeviceClient.CreateFromConnectionString(APPCONTANTS.DeviceConStr, TransportType.Mqtt);
                    s_deviceClient.SetMethodHandlerAsync("DoAction", DoAction, null).Wait();
                    //SendDeviceToCloudMessagesAsync();
                   */
                    SetupMqtt();


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
        static async Task<string> DoAction(string Data)
        {
            //var data = Encoding.UTF8.GetString(Data);
            var action = JsonConvert.DeserializeObject<DeviceAction>(Data);
            // Check the payload is a single integer value
            if (action != null)
            {
                /*
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Telemetry interval set to {0} seconds", data);
                Console.ResetColor();
                */
                switch (action.ActionName)
                {
                    case "PlaySound":
                      
                        break;
                    case "ChangeLED":
                     
                        break;
                    case "TurnOffLED":
               
                        break;
                    case "OpenURL":
                      
                        break;
                    case "CCTVStatus":
                        IsPatrol = Convert.ToBoolean(action.Params[0]);
                        Console.WriteLine($"CCTV Watcher Set to {IsPatrol}");
                        break;
                    case "CCTVUpdateTime":
                        TimeInterval = Convert.ToInt32(action.Params[0]);
                        Console.WriteLine($"CCTV Update Time Set to {TimeInterval} seconds");
                        break;

                }
                // Acknowlege the direct method call with a 200 success message
                string result = "{\"result\":\"Executed direct method: " + action.ActionName + "\"}";
                return result;
                //return new MethodResponse(Encoding.UTF8.GetBytes(result), 200);
            }
            else
            {
                // Acknowlege the direct method call with a 400 error message
                string result = "{\"result\":\"Invalid parameter\"}";
                return result;
                //return new MethodResponse(Encoding.UTF8.GetBytes(result), 400);
            }
        }
        /*
        private static async Task<MethodResponse> DoAction(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            var action = JsonConvert.DeserializeObject<DeviceAction>(data);
            // Check the payload is a single integer value
            if (action != null)
            {
             
                switch (action.ActionName)
                {
                    case "PlaySound":
                
                    case "ChangeLED":
                  
                    case "TurnOffLED":
                    
                    case "OpenURL":
                        //do nothing
                        break;
                    case "CCTVStatus":
                        IsPatrol = Convert.ToBoolean(action.Params[0]);
                        Console.WriteLine($"CCTV Watcher Set to {IsPatrol}");
                        break;
                    case "CCTVUpdateTime":
                        TimeInterval = Convert.ToInt32(action.Params[0]);
                        Console.WriteLine($"CCTV Update Time Set to {TimeInterval} seconds");
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
        }*/
        protected static CCTVData LogAnalysisResult(ImageAnalysis result,string CCTVName)
        {
            var item = new CCTVData();
            Console.WriteLine($"----------------------------------------------");
            Console.WriteLine($"Waktu Pengambilan : {DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")}");
            Console.WriteLine($"Hasil Analisa {CCTVName}:");
            item.CCTVName = CCTVName;
            string Speak = string.Empty;
            if (result == null)
            {
                Log("null");
                return null;
            }

            if (result.Metadata != null)
            {
                Log("Image Format : " + result.Metadata.Format);
                Log("Image Dimensions : " + result.Metadata.Width + " x " + result.Metadata.Height);
            }

            if (result.ImageType != null)
            {
                string clipArtType;
                switch (result.ImageType.ClipArtType)
                {
                    case 0:
                        clipArtType = "0 Non-clipart";
                        break;
                    case 1:
                        clipArtType = "1 ambiguous";
                        break;
                    case 2:
                        clipArtType = "2 normal-clipart";
                        break;
                    case 3:
                        clipArtType = "3 good-clipart";
                        break;
                    default:
                        clipArtType = "Unknown";
                        break;
                }
                Log("Clip Art Type : " + clipArtType);

                string lineDrawingType;
                switch (result.ImageType.LineDrawingType)
                {
                    case 0:
                        lineDrawingType = "0 Non-LineDrawing";
                        break;
                    case 1:
                        lineDrawingType = "1 LineDrawing";
                        break;
                    default:
                        lineDrawingType = "Unknown";
                        break;
                }
                Log("Line Drawing Type : " + lineDrawingType);
            }


            if (result.Adult != null)
            {
                Log("Is Adult Content : " + result.Adult.IsAdultContent);
                Log("Adult Score : " + result.Adult.AdultScore);
                Log("Is Racy Content : " + result.Adult.IsRacyContent);
                Log("Racy Score : " + result.Adult.RacyScore);
            }

            if (result.Categories != null && result.Categories.Count > 0)
            {
                Log("Categories : ");
                foreach (var category in result.Categories)
                {
                    Log("   Name : " + category.Name + "; Score : " + category.Score);
                }
            }

            if (result.Faces != null && result.Faces.Count > 0)
            {
                Log("Faces : ");
                foreach (var face in result.Faces)
                {
                    Log("   Age : " + face.Age + "; Gender : " + face.Gender);
                }
            }

            if (result.Color != null)
            {
                Log("AccentColor : " + result.Color.AccentColor);
                Log("Dominant Color Background : " + result.Color.DominantColorBackground);
                Log("Dominant Color Foreground : " + result.Color.DominantColorForeground);

                if (result.Color.DominantColors != null && result.Color.DominantColors.Count > 0)
                {
                    string colors = "Dominant Colors : ";
                    foreach (var color in result.Color.DominantColors)
                    {
                        colors += color + " ";
                    }
                    Log(colors);
                }
            }

            if (result.Description != null)
            {
                Log("Description : ");
                item.Description = "";
                foreach (var caption in result.Description.Captions)
                {
                    Log("   Caption : " + caption.Text + "; Confidence : " + caption.Confidence);
                    Speak += caption.Text;
                    item.Description += caption.Text+". ";
                }
                string tags = "   Tags : ";
                
                foreach (var tag in result.Description.Tags)
                {
                    tags += tag + ", ";
                   
                }
                Log(tags);

            }

            if (result.Tags != null)
            {
                Log("Tags : ");
                item.Tags = "";
                foreach (var tag in result.Tags)
                {
                    Log("   Name : " + tag.Name + "; Confidence : " + tag.Confidence + "; Hint : " + tag.Hint);
                    item.Tags += tag.Name + ";";
                }
            }
            return item;
        }
        // Analyze a remote image
        private static async Task AnalyzeRemoteAsync(string CCTVName,
             string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine(
                    "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                return;
            }

            ImageAnalysis analysis =
                await computerVision.AnalyzeImageAsync(imageUrl, features);
            LogAnalysisResult(analysis, imageUrl);
        }
        public static void Log(string Pesan)
        {
            Console.WriteLine(Pesan);
        }
        // Analyze a local image
        private static async Task AnalyzeLocalAsync(string CCTVName,
           string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                    imageStream, features);
                var data = LogAnalysisResult(analysis, imagePath);
                foreach(var key in keywords)
                {
                    if (data.Tags.ToLower().Contains(key) || data.Description.ToLower().Contains(key))
                    {
                        //jika terdapat manusia
                        data.Tanggal = DateTime.Now;
                        var res = await blobHelper.UploadFile(imagePath, CCTVName);
                        data.ImageUrl = res.url;
                        data.CCTVName = CCTVName;
                        data.AssignKey();
                        var res2 = await tableHelper.InsertData(data);
                        break;
                    }
                }
            }
        }
        private static async Task AnalyzeStreamAsync(string CCTVName, Stream imageStream)
        {
            try
            {
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                    imageStream, features);
                imageStream.Dispose();
                LogAnalysisResult(analysis, CCTVName);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message + "_" + ex.StackTrace); }
        }
        // Display the most relevant caption for the image
        /*
        private static void DisplayResults(ImageAnalysis analysis, string CCTVName)
        {
            Console.WriteLine($"----------------------------------------------");
            Console.WriteLine($"Waktu Pengambilan : {DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")}");
            Console.WriteLine($"Hasil Analisa {CCTVName}:");
            Console.WriteLine("Caption:"+analysis.Description.Captions[0].Text );
            var TagStr = "";
            new List<ImageTag>(analysis.Tags).ForEach(x => { TagStr += x.Name + ","; });
            Console.WriteLine("Tags:" + TagStr + "\n");
        }
        */
        static async void Watcher()
        {
            Random rnd = new Random(Environment.TickCount);
            while (true)
            {
                if (IsPatrol)
                {
                    for (int x = 0; x < cctvCount; x++)
                    {
                        var bmp = await GetSnapshot(cctvUrl[x] + rnd.NextDouble(), x);
                    }
                }
                Thread.Sleep(TimeInterval*1000);
            }
        }

        static async Task<Bitmap> GetSnapshot(string UrlCCTV,int CCTVID)
        {
            try
            {
                var bmp = await client.GetAsync(UrlCCTV);
                var res = await bmp.Content.ReadAsStreamAsync();
                Bitmap cctvImage = new Bitmap(res);
                var pathimg = cctvpath + $"cctv{CCTVID}.jpg";
                cctvImage.Save(pathimg);
                await AnalyzeLocalAsync($"CCTV {CCTVID}", pathimg);
                res.Dispose();
                return cctvImage;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"error getting cctv {CCTVID} frame :"+ex.Message);
                return null;
            }
        }
    }
    public class DeviceAction
    {
        public string ActionName { get; set; }
        public string[] Params { get; set; }
    }

}
