using System;
using uPLibrary.Networking.M2Mqtt;

namespace BMC.Mqtt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start MQTT!");
            // create and start broker
            MqttBroker broker = new MqttBroker();
            broker.UserAuth = CheckUser;
           
            broker.ClientDisconnected += Broker_ClientDisconnected;
            broker.ClientConnected += Broker_ClientConnected;
            broker.Start();
            Console.WriteLine("MQTT service is started. any key to stop..");
            //Once the broker is started, you applciaiton is free to do whatever it wants. 
            Console.ReadLine();

            ///Stop broker
            broker.Stop();
        }

        private static void Broker_ClientDisconnected(uPLibrary.Networking.M2Mqtt.IntegrationAPI.ClientModel obj)
        {
            Console.WriteLine($"{obj.ClientId} is disconnected");
        }

        private static void Broker_ClientConnected(uPLibrary.Networking.M2Mqtt.IntegrationAPI.ClientModel obj)
        {
            Console.WriteLine($"{obj.ClientId} is connected");
        }

        static bool CheckUser(string username,string password)
        {
            return true;
        }
    }
}
