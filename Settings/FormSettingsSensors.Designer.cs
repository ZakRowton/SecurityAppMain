namespace Security.Settings
{
    partial class FormSettingsSensors
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listBoxSensors = new ListBox();
            label1 = new Label();
            label2 = new Label();
            buttonNewSenor = new Button();
            panelCameraSettings = new Panel();
            textBoxCameraName = new TextBox();
            textBoxIPAddress = new TextBox();
            comboBoxRoom = new ComboBox();
            label7 = new Label();
            label4 = new Label();
            label3 = new Label();
            buttonAddCamera = new Button();
            buttonRemoveCam = new Button();
            comboBoxAvailSenors = new ComboBox();
            buttonRefreshSensorsAvailable = new Button();
            panelCameraSettings.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxSensors
            // 
            listBoxSensors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxSensors.BackColor = Color.FromArgb(75, 75, 75);
            listBoxSensors.BorderStyle = BorderStyle.FixedSingle;
            listBoxSensors.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listBoxSensors.ForeColor = SystemColors.Window;
            listBoxSensors.FormattingEnabled = true;
            listBoxSensors.ItemHeight = 27;
            listBoxSensors.Location = new Point(40, 88);
            listBoxSensors.Name = "listBoxSensors";
            listBoxSensors.Size = new Size(307, 407);
            listBoxSensors.TabIndex = 0;
            listBoxSensors.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tempus Sans ITC", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(971, 9);
            label1.Name = "label1";
            label1.Size = new Size(231, 42);
            label1.TabIndex = 1;
            label1.Text = "Sensor Settings";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(34, 48);
            label2.Name = "label2";
            label2.Size = new Size(81, 27);
            label2.TabIndex = 2;
            label2.Text = "Sensors:";
            // 
            // buttonNewSenor
            // 
            buttonNewSenor.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonNewSenor.BackColor = Color.FromArgb(75, 75, 75);
            buttonNewSenor.FlatStyle = FlatStyle.Popup;
            buttonNewSenor.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonNewSenor.ForeColor = SystemColors.ButtonHighlight;
            buttonNewSenor.Location = new Point(40, 529);
            buttonNewSenor.Name = "buttonNewSenor";
            buttonNewSenor.Size = new Size(250, 38);
            buttonNewSenor.TabIndex = 3;
            buttonNewSenor.Text = "ADD NEW SENSOR";
            buttonNewSenor.UseVisualStyleBackColor = false;
            buttonNewSenor.Click += buttonNewCamera_Click;
            // 
            // panelCameraSettings
            // 
            panelCameraSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelCameraSettings.Controls.Add(textBoxCameraName);
            panelCameraSettings.Controls.Add(textBoxIPAddress);
            panelCameraSettings.Controls.Add(comboBoxRoom);
            panelCameraSettings.Controls.Add(label7);
            panelCameraSettings.Controls.Add(label4);
            panelCameraSettings.Controls.Add(label3);
            panelCameraSettings.Location = new Point(376, 88);
            panelCameraSettings.Name = "panelCameraSettings";
            panelCameraSettings.Size = new Size(804, 422);
            panelCameraSettings.TabIndex = 4;
            panelCameraSettings.Visible = false;
            // 
            // textBoxCameraName
            // 
            textBoxCameraName.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxCameraName.Location = new Point(25, 46);
            textBoxCameraName.Name = "textBoxCameraName";
            textBoxCameraName.Size = new Size(336, 35);
            textBoxCameraName.TabIndex = 12;
            // 
            // textBoxIPAddress
            // 
            textBoxIPAddress.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxIPAddress.Location = new Point(25, 134);
            textBoxIPAddress.Name = "textBoxIPAddress";
            textBoxIPAddress.Size = new Size(336, 35);
            textBoxIPAddress.TabIndex = 11;
            // 
            // comboBoxRoom
            // 
            comboBoxRoom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBoxRoom.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            comboBoxRoom.FormattingEnabled = true;
            comboBoxRoom.Location = new Point(442, 46);
            comboBoxRoom.Name = "comboBoxRoom";
            comboBoxRoom.Size = new Size(311, 35);
            comboBoxRoom.TabIndex = 8;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.ButtonHighlight;
            label7.Location = new Point(442, 16);
            label7.Name = "label7";
            label7.Size = new Size(173, 27);
            label7.TabIndex = 7;
            label7.Text = "Designated Room:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.ButtonHighlight;
            label4.Location = new Point(25, 104);
            label4.Name = "label4";
            label4.Size = new Size(114, 27);
            label4.TabIndex = 4;
            label4.Text = "IP-Address:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ButtonHighlight;
            label3.Location = new Point(25, 16);
            label3.Name = "label3";
            label3.Size = new Size(71, 27);
            label3.TabIndex = 3;
            label3.Text = "Name:";
            // 
            // buttonAddCamera
            // 
            buttonAddCamera.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonAddCamera.BackColor = Color.FromArgb(75, 75, 75);
            buttonAddCamera.FlatStyle = FlatStyle.Popup;
            buttonAddCamera.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAddCamera.ForeColor = SystemColors.ButtonHighlight;
            buttonAddCamera.Location = new Point(956, 529);
            buttonAddCamera.Name = "buttonAddCamera";
            buttonAddCamera.Size = new Size(224, 38);
            buttonAddCamera.TabIndex = 5;
            buttonAddCamera.Text = "ADD SENSOR";
            buttonAddCamera.UseVisualStyleBackColor = false;
            buttonAddCamera.Visible = false;
            buttonAddCamera.Click += buttonAddCamera_Click;
            // 
            // buttonRemoveCam
            // 
            buttonRemoveCam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonRemoveCam.BackColor = Color.FromArgb(75, 75, 75);
            buttonRemoveCam.FlatStyle = FlatStyle.Popup;
            buttonRemoveCam.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonRemoveCam.ForeColor = SystemColors.ButtonHighlight;
            buttonRemoveCam.Location = new Point(716, 529);
            buttonRemoveCam.Name = "buttonRemoveCam";
            buttonRemoveCam.Size = new Size(224, 38);
            buttonRemoveCam.TabIndex = 6;
            buttonRemoveCam.Text = "REMOVE SENSOR";
            buttonRemoveCam.UseVisualStyleBackColor = false;
            buttonRemoveCam.Visible = false;
            buttonRemoveCam.Click += buttonRemoveCam_Click;
            // 
            // comboBoxAvailSenors
            // 
            comboBoxAvailSenors.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            comboBoxAvailSenors.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            comboBoxAvailSenors.FormattingEnabled = true;
            comboBoxAvailSenors.Location = new Point(305, 529);
            comboBoxAvailSenors.Name = "comboBoxAvailSenors";
            comboBoxAvailSenors.Size = new Size(280, 35);
            comboBoxAvailSenors.TabIndex = 9;
            // 
            // buttonRefreshSensorsAvailable
            // 
            buttonRefreshSensorsAvailable.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonRefreshSensorsAvailable.BackColor = Color.FromArgb(75, 75, 75);
            buttonRefreshSensorsAvailable.BackgroundImage = Properties.Resources.refresh;
            buttonRefreshSensorsAvailable.BackgroundImageLayout = ImageLayout.Stretch;
            buttonRefreshSensorsAvailable.FlatStyle = FlatStyle.Popup;
            buttonRefreshSensorsAvailable.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonRefreshSensorsAvailable.ForeColor = SystemColors.ButtonHighlight;
            buttonRefreshSensorsAvailable.Location = new Point(591, 529);
            buttonRefreshSensorsAvailable.Name = "buttonRefreshSensorsAvailable";
            buttonRefreshSensorsAvailable.Size = new Size(41, 38);
            buttonRefreshSensorsAvailable.TabIndex = 10;
            buttonRefreshSensorsAvailable.UseVisualStyleBackColor = false;
            buttonRefreshSensorsAvailable.Click += buttonRefreshSensorsAvailable_Click;
            // 
            // FormSettingsSensors
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1214, 586);
            Controls.Add(buttonRefreshSensorsAvailable);
            Controls.Add(comboBoxAvailSenors);
            Controls.Add(buttonRemoveCam);
            Controls.Add(buttonAddCamera);
            Controls.Add(panelCameraSettings);
            Controls.Add(buttonNewSenor);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBoxSensors);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormSettingsSensors";
            Text = "FormSettingsCameras";
            Load += FormSettingsCameras_Load;
            panelCameraSettings.ResumeLayout(false);
            panelCameraSettings.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBoxSensors;
        private Label label1;
        private Label label2;
        private Button buttonNewSenor;
        private Panel panelCameraSettings;
        private Label label3;
        private TextBox textBoxCameraName;
        private TextBox textBoxIPAddress;
        private ComboBox comboBoxRoom;
        private Label label7;
        private Label label4;
        private Button buttonAddCamera;
        private Button buttonRemoveCam;
        private ComboBox comboBoxAvailSenors;
        private Button buttonRefreshSensorsAvailable;
    }
}