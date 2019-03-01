using System;
using System.Collections.Generic;
using System.Text;

namespace BMC.Sarah
{
    public class DeviceData
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }

        public static List<DeviceData> GetAllDevices()
        {
            return new List<DeviceData>()
            {
                new DeviceData (){ ID=1, Name="Kitchen Lamp", IP="192.168.1.27" },
                new DeviceData (){ ID=2, Name="Inspiration Room Lamp", IP="192.168.1.25" },
                new DeviceData (){ ID=3, Name="Living Room Lamp", IP="192.168.1.28" },
                new DeviceData (){ ID=4, Name="Prayer Room Fan", IP="192.168.1.31" },
                new DeviceData (){ ID=5, Name="Inspiration Room Fan", IP="192.168.1.32" },
                new DeviceData (){ ID=6, Name="Kitchen Fan", IP="192.168.1.33" },
                //new DeviceData (){ ID=7, Name="Lampu Musolla", IP="192.168.1.35" },
                new DeviceData (){ ID=7, Name="Guest Room Lamp", IP="192.168.1.26" },
                new DeviceData (){ ID=8, Name="Front Room Fan", IP="192.168.1.36" },

                   new DeviceData (){ ID=9, Name="Prayer Room Lamp", IP="192.168.1.29" }
            };
        }
    }
}
