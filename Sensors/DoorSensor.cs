using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Sensors
{
    public class DoorSensor
    {
        public String SensorName { get; set; }
        public String IPAddress { get; set; }
        public String RoomID { get; set; } = "";
        public DoorSensor() { }
        public DoorSensor(string sensorName, string ipAddress)
        {
            SensorName = sensorName;
            IPAddress = ipAddress;
        }
    }
}
