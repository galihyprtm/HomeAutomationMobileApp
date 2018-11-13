using BMC.Security.Models;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        private static ServiceClient s_serviceClient;

        // Connection string for your IoT Hub
        // az iot hub show-connection-string --hub-name {your iot hub name}
        private readonly static string s_connectionString = "HostName=FreeDeviceHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=pGREIqFsT9rGgDkGJP3K5Vkrg5zmTnNZAxNeqWpT4UM=";

        // Invoke the direct method on the device, passing the payload
        private static async Task InvokeMethod()
        {
            var methodInvocation = new CloudToDeviceMethod("DoAction") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            var action = new DeviceAction() { ActionName = "PlaySound", Params = new string []{"monster.mp3" } };
            methodInvocation.SetPayloadJson(JsonConvert.SerializeObject(action));

            // Invoke the direct method asynchronously and get the response from the simulated device.
            var response = await s_serviceClient.InvokeDeviceMethodAsync("BMCSecurityBot", methodInvocation);

            Console.WriteLine("Response status: {0}, payload:", response.Status);
            Console.WriteLine(response.GetPayloadAsJson());
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("IoT Hub Quickstarts #2 - Back-end application.\n");

            // Create a ServiceClient to communicate with service-facing endpoint on your hub.
            s_serviceClient = ServiceClient.CreateFromConnectionString(s_connectionString);
            InvokeMethod().GetAwaiter().GetResult();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
