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
        Form activeChildForm = null;
        FormSettingsCameras FormCameras = new FormSettingsCameras();
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
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
    }
}
