using Security.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Security.Settings
{
    public partial class FormSettingsSensors : Form
    {
        private Storage Storage { get; set; }
        private string previousSensorName = "";
        DoorSensor SelectedDoorSensor { get; set; }
        WindowSensor SelectedWindowSensor { get; set; }
        public FormSettingsSensors(Storage storage)
        {
            InitializeComponent();
            Storage = storage;
        }

        private void buttonNewCamera_Click(object sender, EventArgs e)
        {
            SelectedDoorSensor = new DoorSensor();
            SelectedWindowSensor = new WindowSensor();
            panelCameraSettings.Visible = true;
            buttonAddCamera.Visible = true;
            buttonRemoveCam.Visible = true;
            textBoxCameraName.Text = "";
            textBoxIPAddress.Text = "192.168.0.";
            previousSensorName = "";

            comboBoxRoom.Items.Clear();
            foreach (Room room in Storage.Rooms)
            {
                comboBoxRoom.Items.Add(room.Name);
            }
        }

        private void buttonAddCamera_Click(object sender, EventArgs e)
        {
            bool isNew = true;
            Room selectedRoom = null;
            SelectedDoorSensor = new DoorSensor();
            SelectedWindowSensor = new WindowSensor();

            foreach (Room room in Storage.Rooms)
                if (room.Name == comboBoxRoom.Text)
                    selectedRoom = room;


            foreach (Camera camera in Storage.Cameras)
            {
                if (camera.Name == previousSensorName)
                {
                    break;
                }
            }

            panelCameraSettings.Visible = false;
            buttonAddCamera.Visible = false;
            buttonRemoveCam.Visible = false;
            SelectedDoorSensor = null;
            previousSensorName = "";
            ReloadSensors();
        }

        private void ReloadSensors()
        {
            SelectedDoorSensor = null;
            previousSensorName = "";
            listBoxSensors.Items.Clear();

            foreach (DoorSensor sensor in Storage.DoorSensors)
                listBoxSensors.Items.Add(sensor.SensorName + " - Door");
            foreach (WindowSensor sensor in Storage.WindowSensors)
                listBoxSensors.Items.Add(sensor.SensorName + " - Window");

            listBoxSensors.SelectedIndex = -1;
        }

        private void buttonRemoveCam_Click(object sender, EventArgs e)
        {
            //foreach (Camera camera in Storage.Cameras)
            //{
            //    if (camera.Name == previousCameraName || camera.Name == SelectedCamera.Name)
            //    {
            //        Storage.Cameras.Remove(camera);
            //        Storage.Save();
            //        break;
            //    }
            //}

            panelCameraSettings.Visible = false;
            buttonAddCamera.Visible = false;
            buttonRemoveCam.Visible = false;
            //SelectedCamera = null;
            previousSensorName = "";
            ReloadSensors();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DoorSensor camera in Storage.DoorSensors)
            {
                if (camera.SensorName == listBoxSensors.Items[listBoxSensors.SelectedIndex].ToString())
                {
                    panelCameraSettings.Visible = true;
                    buttonAddCamera.Visible = true;
                    buttonRemoveCam.Visible = true;

                    textBoxCameraName.Text = camera.SensorName;
                    textBoxIPAddress.Text = camera.IPAddress;

                    previousSensorName = camera.SensorName;

                    for (int i = 0; i < comboBoxRoom.Items.Count; i++)
                    {
                        if (comboBoxRoom.Items[i].ToString() == camera.Room.Name)
                        {
                            comboBoxRoom.SelectedIndex = i;
                            break;
                        }
                    }

                }
            }
        }

        private void FormSettingsCameras_Load(object sender, EventArgs e)
        {
            comboBoxRoom.Items.Clear();
            foreach (Room room in Storage.Rooms)
                comboBoxRoom.Items.Add(room.Name);
            ReloadSensors();
        }

        private void buttonRefreshSensorsAvailable_Click(object sender, EventArgs e)
        {
            AvailableDevices.Clear();
            comboBoxAvailSenors.Items.Clear();
            buttonRefreshSensorsAvailable.Enabled = false;
            buttonNewSenor.Enabled = false;
            ScanForSensors();
            buttonRefreshSensorsAvailable.Enabled = true;
            buttonNewSenor.Enabled = true;
        }

        private List<DoorSensor> AvailableDevices { get; set; } = new List<DoorSensor>();
        private async void ScanForSensors()
        {
            SensorScanner Scanner = new SensorScanner();

            Console.WriteLine("Starting network scan...");
            // Define the range: 192.168.0.100 to 192.168.0.110
            string baseIp = "192.168.0";
            int start = 100;
            int end = 110;
            int scanTimeout = 1000; // milliseconds - adjust as needed

            try
            {
                int sensorCt = 1;
                List<IPAddress> onlineDevices = await Scanner.FindDevicesAsync(baseIp, start, end, scanTimeout);

                foreach (IPAddress onlineDevice in onlineDevices)
                {
                    DoorSensor availableSensor = new DoorSensor();
                    availableSensor.SensorName = "Available Sensor " + sensorCt;
                    availableSensor.IPAddress = onlineDevice.ToString();
                    availableSensor.Room = null;
                    comboBoxAvailSenors.Items.Add(availableSensor.SensorName);
                    AvailableDevices.Add(availableSensor);
                    sensorCt++;
                }

                sensorCt = 1;
                if (comboBoxAvailSenors.Items.Count > 0) comboBoxAvailSenors.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAn error occurred during the scan: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.ResetColor();
            }
        }
    }
}
