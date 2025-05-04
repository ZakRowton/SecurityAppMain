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
    public partial class FormSettings : Form
    {
        private Storage Storage { get; set; }

        Form activeChildForm = null;
        FormSettingsCameras FormCameras = null;
        FormSettingsRooms FormRooms = null;
        FormSettingsSensors FormSensors = null;

        public FormSettings(Storage storage)
        {
            Storage = storage;
            FormCameras = new FormSettingsCameras(Storage);
            FormRooms = new FormSettingsRooms(Storage);
            FormSensors = new FormSettingsSensors(Storage);
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            FormCameras = new FormSettingsCameras(Storage);
            FormRooms = new FormSettingsRooms(Storage);
            FormSensors = new FormSettingsSensors(Storage);
            LoadFormIntoPanel(FormCameras);
        }

        private void LoadFormIntoPanel(Form childForm)
        {
            if (panelSettings == null) { MessageBox.Show("Error: panelSettings is null."); return; }
            if (childForm == null) { MessageBox.Show("Error: childForm is null."); return; }
            if (activeChildForm == childForm) { childForm.BringToFront(); return; }
            activeChildForm?.Close();
            panelSettings.Controls.Clear();
            activeChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelSettings.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
        }

        private void buttonRooms_Click(object sender, EventArgs e)
        {
            FormRooms = new FormSettingsRooms(Storage);
            LoadFormIntoPanel(FormRooms);
        }

        private void btnNavCameras_Click(object sender, EventArgs e)
        {
            FormCameras = new FormSettingsCameras(Storage);
            LoadFormIntoPanel(FormCameras);
        }

        private void buttonSensors_Click(object sender, EventArgs e)
        {
            FormSensors = new FormSettingsSensors(Storage);
            LoadFormIntoPanel(FormSensors);
        }
    }
}
