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
    public partial class FormSettingsRooms : Form
    {
        private Storage Storage { get; set; }
        private string previousName = string.Empty;
        private Room SelectedRoom { get; set; }
        public FormSettingsRooms(Storage storage)
        {
            InitializeComponent();
            Storage = storage;
        }

        private void listBoxRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Room room in Storage.Rooms)
                if (room.Name == listBoxRooms.Items[listBoxRooms.SelectedIndex].ToString())
                    SelectedRoom = room;

            textBoxCameraName.Text = SelectedRoom.Name;

            buttonAddRoom.Visible = true;
            buttonDeleteRoom.Visible = true;
            buttonAddRoom.Text = "Save Room";
            previousName = SelectedRoom.Name;
            panelCameraSettings.Visible = true;
        }

        private void FormSettingsRooms_Load(object sender, EventArgs e)
        {
            ReloadRoomsList();
        }

        private void ReloadRoomsList()
        {
            listBoxRooms.Items.Clear();

            foreach (var room in Storage.Rooms)
                listBoxRooms.Items.Add(room.Name);

            listBoxRooms.SelectedIndex = -1;
        }

        private void buttonNewRoom_Click(object sender, EventArgs e)
        {
            listBoxRooms.SelectedIndex = -1;
            panelCameraSettings.Visible = true;
            buttonAddRoom.Visible = true;
            textBoxCameraName.Text = "";
            SelectedRoom = new Room();
        }

        private void buttonAddRoom_Click(object sender, EventArgs e)
        {
            bool isNew = false;
            SelectedRoom.Name = textBoxCameraName.Text;

            //Check If Room Was Previously In List Or If Need ToAdd New - Remove It Then Re-Add
            foreach (Room room in Storage.Rooms)
            {
                if (room.Name.Equals(SelectedRoom.Name) || room.Name.Equals(previousName))
                {
                    isNew = false;
                    Storage.Rooms.Remove(room);
                    break;
                }
            }

            labelStatus.Text = isNew ?
                $"{textBoxCameraName.Text} Added To Rooms Successfully!" :
                (textBoxCameraName.Text != previousName ?
                    $"{previousName} Updated To {textBoxCameraName.Text} Successfully!" :
                    $"{textBoxCameraName.Text} Updated Successfully!"
                );

            Storage.Rooms.Add(SelectedRoom);
            Storage.Save();
            ReloadRoomsList();
            panelCameraSettings.Visible = false;
            textBoxCameraName.Text = "";
            buttonAddRoom.Visible = false;
            labelStatus.Visible = true;
            timerHideLabels.Enabled = true;
        }

        private async void timerHideLabels_Tick(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            labelStatus.Visible = false;
            timerHideLabels.Enabled = false;
        }

        private void buttonDeleteRoom_Click(object sender, EventArgs e)
        {
            Room deletedRoom = null;

            foreach (Room room in Storage.Rooms)
            {
                if (room.Name == listBoxRooms.Items[listBoxRooms.SelectedIndex].ToString())
                {
                    deletedRoom = room;
                    Storage.Rooms.Remove(room);
                    Storage.Save();
                    break;
                }
            }

            panelCameraSettings.Visible = false;
            textBoxCameraName.Text = "";
            buttonAddRoom.Visible = false;
            buttonDeleteRoom.Visible = false;
            ReloadRoomsList();
            labelStatus.Text = $"{deletedRoom.Name} Removed From Rooms!";
            timerHideLabels.Enabled = true;
        }
    }
}
