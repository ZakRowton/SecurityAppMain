using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Sensors
{
    public class WindowSensor
    {
        public String SensorName { get; set; }
        public String IPAddress { get; set; }
        public String RoomID { get; set; } = "";
        public WindowSensor() { }
        public WindowSensor(string sensorName, string ipAddress)
        {
            SensorName = sensorName;
            IPAddress = ipAddress;
        }
    }
}
