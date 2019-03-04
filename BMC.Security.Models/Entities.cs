using System;

namespace BMC.Security.Models
{
    public class Entities
    {
    }
    public class DeviceAction
    {
        public string ActionName { get; set; }
        public string[] Params { get; set; }
    }

    public class EnvData
    {
        public (double x,double y,double z) Accel { get; set; }
        public DateTime LocalTime { get; set; }
        public double Light { get; set; }
        public double Temp { get; set; }
    }
}
