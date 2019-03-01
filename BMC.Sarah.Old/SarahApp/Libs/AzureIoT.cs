using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SarahApp.Libs
{
    public class AzureIoT
    {
        private static ServiceClient s_serviceClient;
        public AzureIoT()
        {

            s_serviceClient = ServiceClient.CreateFromConnectionString(s_connectionString);

        }
        // Connection string for your IoT Hub
        // az iot hub show-connection-string --hub-name {your iot hub name}
        private readonly static string s_connectionString = APPCONTANTS.AzureIoTCon;

        // Invoke the direct method on the device, passing the payload
        public async Task InvokeMethod(string DeviceId, string ActionName = "PlaySound", params string[] Params)
        {
            var methodInvocation = new CloudToDeviceMethod("DoAction") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            var action = new DeviceAction() { ActionName = ActionName, Params = Params };
            methodInvocation.SetPayloadJson(JsonConvert.SerializeObject(action));

            // Invoke the direct method asynchronously and get the response from the simulated device.
            var response = await s_serviceClient.InvokeDeviceMethodAsync(DeviceId, methodInvocation);

            Console.WriteLine("Response status: {0}, payload:", response.Status);
            Console.WriteLine(response.GetPayloadAsJson());
        }


    }
    public class DeviceAction
    {
        public string ActionName { get; set; }
        public string[] Params { get; set; }
    }
}
