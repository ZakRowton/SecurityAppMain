using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Security.Settings
{
    public partial class FormSettingsCameras : Form
    {
        private Storage Storage { get; set; }
        private string previousCameraName = "";
        Camera SelectedCamera { get; set; }
        public FormSettingsCameras(Storage storage)
        {
            InitializeComponent();
            Storage = storage;
        }

        private void buttonNewCamera_Click(object sender, EventArgs e)
        {
            SelectedCamera = new Camera();
            panelCameraSettings.Visible = true;
            buttonAddCamera.Visible = true;
            buttonRemoveCam.Visible = true;
            textBoxCameraName.Text = "";
            textBoxIPAddress.Text = "192.168.0.";
            textBoxWyzePass.Text = "";
            textBoxWyzeUser.Text = "";
            previousCameraName = "";

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
            SelectedCamera = new Camera();

            foreach (Room room in Storage.Rooms)
                if (room.Name == comboBoxRoom.Text)
                    selectedRoom = room;

            SelectedCamera.Name = textBoxCameraName.Text;
            SelectedCamera.IPAddress = textBoxIPAddress.Text;
            SelectedCamera.Room = selectedRoom;
            SelectedCamera.WyzeUsername = textBoxWyzeUser.Text;
            SelectedCamera.WyzePassword = textBoxWyzePass.Text;

            foreach (Camera camera in Storage.Cameras)
            {
                if (camera.Name == previousCameraName || camera.Name == SelectedCamera.Name)
                {
                    isNew = false;
                    Storage.Cameras.Remove(camera);
                    Storage.Cameras.Add(SelectedCamera);
                    Storage.Save();
                    break;
                }
            }

            panelCameraSettings.Visible = false;
            buttonAddCamera.Visible = false;
            buttonRemoveCam.Visible = false;
            SelectedCamera = null;
            previousCameraName = "";
            ReloadCameras();
        }

        private void ReloadCameras()
        {
            SelectedCamera = null;
            previousCameraName = "";
            listBox1.Items.Clear();

            foreach (Camera camera in Storage.Cameras)
                listBox1.Items.Add(camera.Name);

            listBox1.SelectedIndex = -1;
        }

        private void buttonRemoveCam_Click(object sender, EventArgs e)
        {
            foreach (Camera camera in Storage.Cameras)
            {
                if (camera.Name == previousCameraName || camera.Name == SelectedCamera.Name)
                {
                    Storage.Cameras.Remove(camera);
                    Storage.Save();
                    break;
                }
            }

            panelCameraSettings.Visible = false;
            buttonAddCamera.Visible = false;
            buttonRemoveCam.Visible = false;
            SelectedCamera = null;
            previousCameraName = "";
            ReloadCameras();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Camera camera in Storage.Cameras)
            {
                if (camera.Name == listBox1.Items[listBox1.SelectedIndex].ToString())
                {
                    panelCameraSettings.Visible = true;
                    buttonAddCamera.Visible = true;
                    buttonRemoveCam.Visible = true;

                    textBoxCameraName.Text = camera.Name;
                    textBoxIPAddress.Text = camera.IPAddress;
                    textBoxWyzeUser.Text = camera.WyzeUsername;
                    textBoxWyzePass.Text = camera.WyzePassword;

                    previousCameraName = camera.Name;

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
            ReloadCameras();
        }
    }
}
