﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using BMC.Security.Models;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;

namespace BMC.Security.MobileApp.Helpers
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
        public void SendCommand(string Message, string Topic)
        {
            MqttClient.Publish(Topic, Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        void SetupMqtt()
        {
            string IPBrokerAddress = ConfigurationManager.AppSettings["MqttHost"];
            string ClientUser = ConfigurationManager.AppSettings["MqttUser"];
            string ClientPass = ConfigurationManager.AppSettings["MqttPass"];

            MqttClient = new MqttClient(IPBrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            MqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            var clientId = "bmc-home-web";//Guid.NewGuid().ToString();

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
        public Task InvokeMethod(string DeviceId, string ActionName, params string[] Params)
        {
            return Task.Factory.StartNew(() =>
            {
                var action = new DeviceAction() { ActionName = ActionName, Params = Params };
                SendCommand(JsonConvert.SerializeObject(action), ControlTopic);
            });
            //Console.WriteLine("Response status: {0}, payload:", response.Status);
            //Console.WriteLine(response.GetPayloadAsJson());
        }

        public Task InvokeMethod2(string Topic, string ActionName, params string[] Params)
        {
            return Task.Factory.StartNew(() => {
                var action = new DeviceAction() { ActionName = ActionName, Params = Params };
                SendCommand(JsonConvert.SerializeObject(action), Topic);
            });

            //Console.WriteLine("Response status: {0}, payload:", response.Status);
            //Console.WriteLine(response.GetPayloadAsJson());
        }
    }
}
