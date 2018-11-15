using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BMC.Security.CCTV
{
    class Program
    {// subscriptionKey = "0123456789abcdef0123456789ABCDEF"
        private const string subscriptionKey = "ed9496df54b54bca9a46ad451065b604";
        static int cctvCount = 2;
        static string[] cctvUrl = new string[] { "http://192.168.1.180:80/snapshot?chn=1&u=admin&p=&q=0&d=1&rand=", "http://192.168.1.88:80/snapshot?chn=1&u=admin&p=&q=0&d=1&rand=" };
        const string cctvpath = @"C:\Users\mifma\Documents\temp\";
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
        static HttpClient client;
        static ComputerVisionClient computerVision;
        static void Main(string[] args)
        {
            if (client == null) client = new HttpClient();
            computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });
            computerVision.Endpoint = "https://southeastasia.api.cognitive.microsoft.com";

            Watcher();
            Console.ReadLine();
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
            DisplayResults(analysis, imageUrl);
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
                DisplayResults(analysis, imagePath);
            }
        }
        private static async Task AnalyzeStreamAsync(string CCTVName,
            Stream imageStream)
        {
            try
            {
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                    imageStream, features);
                imageStream.Dispose();
                DisplayResults(analysis, CCTVName);
            }catch(Exception ex)
            { Console.WriteLine(ex.Message +"_"+ex.StackTrace); }
        }
        // Display the most relevant caption for the image
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
        static async void Watcher()
        {
            Random rnd = new Random(Environment.TickCount);
            while (true)
            {
                for (int x = 0; x < cctvCount; x++)
                {
                    var bmp = await GetSnapshot(cctvUrl[x]+ rnd.Next(0, 100),x);
                }
                Thread.Sleep(5000);
            }
        }

        static async Task<Bitmap> GetSnapshot(string UrlCCTV,int CCTVID)
        {
            var bmp = await client.GetAsync(UrlCCTV);
            var res = await bmp.Content.ReadAsStreamAsync();
            Bitmap cctvImage = new Bitmap(res);
            var pathimg = cctvpath + $"cctv{CCTVID}.jpg";
            cctvImage.Save(pathimg);
            await AnalyzeLocalAsync($"CCTV {CCTVID}",pathimg);
            res.Dispose();
            return cctvImage;
        }
    }
}
