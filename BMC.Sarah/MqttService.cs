using BMC.Security.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace BMC.Sarah
{
    public class MqttService
    {
        public MqttService()
        {
            SetupMqtt();
        }
        MqttClient MqttClient;
        const string DataTopic = "bmc/homeautomation/data";
        const string ControlTopic = "bmc/homeautomation/control";
        public void PublishMessage(string Message)
        {
            MqttClient.Publish(DataTopic, Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void SendCommand(string Message)
        {
            MqttClient.Publish(ControlTopic, Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        void SetupMqtt()
        {
            string IPBrokerAddress = CONSTANTS.MqttHost;
            string ClientUser = CONSTANTS.MqttUser;
            string ClientPass = CONSTANTS.MqttPass;

            MqttClient = new MqttClient(IPBrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            MqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            var clientId = "bmc-sarah";//Guid.NewGuid().ToString();

            MqttClient.Connect(clientId, ClientUser, ClientPass);
            Console.WriteLine("MQTT is connected");
        } // this code runs when a message was received
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            if (e.Topic == ControlTopic)
            {
                Console.WriteLine(ReceivedMessage);
            }
        }
        // Invoke the direct method on the device, passing the payload
        public Task InvokeMethod(string DeviceId, string ActionName = "PlaySound", params string[] Params)
        {
            return Task.Factory.StartNew(() => {
                var action = new DeviceAction() { ActionName = ActionName, Params = Params };

                SendCommand(JsonConvert.SerializeObject(action));
            });

            //Console.WriteLine("Response status: {0}, payload:", response.Status);
            //Console.WriteLine(response.GetPayloadAsJson());
        }


    }
}
