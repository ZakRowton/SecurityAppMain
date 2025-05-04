namespace Security.Settings
{
    partial class FormSettingsCameras
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
            listBox1 = new ListBox();
            label1 = new Label();
            label2 = new Label();
            buttonNewCamera = new Button();
            panelCameraSettings = new Panel();
            textBoxCameraName = new TextBox();
            textBoxIPAddress = new TextBox();
            textBoxWyzePass = new TextBox();
            textBoxWyzeUser = new TextBox();
            comboBoxRoom = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            buttonAddCamera = new Button();
            buttonRemoveCam = new Button();
            panelCameraSettings.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.BackColor = Color.FromArgb(75, 75, 75);
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            listBox1.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listBox1.ForeColor = SystemColors.Window;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 27;
            listBox1.Location = new Point(40, 88);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(307, 407);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tempus Sans ITC", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(956, 9);
            label1.Name = "label1";
            label1.Size = new Size(246, 42);
            label1.TabIndex = 1;
            label1.Text = "Camera Settings";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(34, 48);
            label2.Name = "label2";
            label2.Size = new Size(90, 27);
            label2.TabIndex = 2;
            label2.Text = "Cameras:";
            // 
            // buttonNewCamera
            // 
            buttonNewCamera.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonNewCamera.BackColor = Color.FromArgb(75, 75, 75);
            buttonNewCamera.FlatStyle = FlatStyle.Popup;
            buttonNewCamera.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonNewCamera.ForeColor = SystemColors.ButtonHighlight;
            buttonNewCamera.Location = new Point(40, 529);
            buttonNewCamera.Name = "buttonNewCamera";
            buttonNewCamera.Size = new Size(307, 38);
            buttonNewCamera.TabIndex = 3;
            buttonNewCamera.Text = "NEW CAMERA";
            buttonNewCamera.UseVisualStyleBackColor = false;
            buttonNewCamera.Click += buttonNewCamera_Click;
            // 
            // panelCameraSettings
            // 
            panelCameraSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelCameraSettings.Controls.Add(textBoxCameraName);
            panelCameraSettings.Controls.Add(textBoxIPAddress);
            panelCameraSettings.Controls.Add(textBoxWyzePass);
            panelCameraSettings.Controls.Add(textBoxWyzeUser);
            panelCameraSettings.Controls.Add(comboBoxRoom);
            panelCameraSettings.Controls.Add(label7);
            panelCameraSettings.Controls.Add(label6);
            panelCameraSettings.Controls.Add(label5);
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
            // textBoxWyzePass
            // 
            textBoxWyzePass.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxWyzePass.Location = new Point(25, 321);
            textBoxWyzePass.Name = "textBoxWyzePass";
            textBoxWyzePass.Size = new Size(336, 35);
            textBoxWyzePass.TabIndex = 10;
            // 
            // textBoxWyzeUser
            // 
            textBoxWyzeUser.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxWyzeUser.Location = new Point(25, 250);
            textBoxWyzeUser.Name = "textBoxWyzeUser";
            textBoxWyzeUser.Size = new Size(336, 35);
            textBoxWyzeUser.TabIndex = 9;
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
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = SystemColors.ButtonHighlight;
            label6.Location = new Point(25, 291);
            label6.Name = "label6";
            label6.Size = new Size(152, 27);
            label6.TabIndex = 6;
            label6.Text = "Wyze Password:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ButtonHighlight;
            label5.Location = new Point(25, 220);
            label5.Name = "label5";
            label5.Size = new Size(161, 27);
            label5.TabIndex = 5;
            label5.Text = "Wyze Username:";
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
            buttonAddCamera.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonAddCamera.BackColor = Color.FromArgb(75, 75, 75);
            buttonAddCamera.FlatStyle = FlatStyle.Popup;
            buttonAddCamera.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAddCamera.ForeColor = SystemColors.ButtonHighlight;
            buttonAddCamera.Location = new Point(956, 529);
            buttonAddCamera.Name = "buttonAddCamera";
            buttonAddCamera.Size = new Size(224, 38);
            buttonAddCamera.TabIndex = 5;
            buttonAddCamera.Text = "ADD CAMERA";
            buttonAddCamera.UseVisualStyleBackColor = false;
            buttonAddCamera.Visible = false;
            buttonAddCamera.Click += buttonAddCamera_Click;
            // 
            // buttonRemoveCam
            // 
            buttonRemoveCam.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonRemoveCam.BackColor = Color.FromArgb(75, 75, 75);
            buttonRemoveCam.FlatStyle = FlatStyle.Popup;
            buttonRemoveCam.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonRemoveCam.ForeColor = SystemColors.ButtonHighlight;
            buttonRemoveCam.Location = new Point(716, 529);
            buttonRemoveCam.Name = "buttonRemoveCam";
            buttonRemoveCam.Size = new Size(224, 38);
            buttonRemoveCam.TabIndex = 6;
            buttonRemoveCam.Text = "REMOVE CAMERA";
            buttonRemoveCam.UseVisualStyleBackColor = false;
            buttonRemoveCam.Visible = false;
            buttonRemoveCam.Click += buttonRemoveCam_Click;
            // 
            // FormSettingsCameras
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1214, 586);
            Controls.Add(buttonRemoveCam);
            Controls.Add(buttonAddCamera);
            Controls.Add(panelCameraSettings);
            Controls.Add(buttonNewCamera);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBox1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormSettingsCameras";
            Text = "FormSettingsCameras";
            Load += FormSettingsCameras_Load;
            panelCameraSettings.ResumeLayout(false);
            panelCameraSettings.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Label label1;
        private Label label2;
        private Button buttonNewCamera;
        private Panel panelCameraSettings;
        private Label label3;
        private TextBox textBoxCameraName;
        private TextBox textBoxIPAddress;
        private TextBox textBoxWyzePass;
        private TextBox textBoxWyzeUser;
        private ComboBox comboBoxRoom;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Button buttonAddCamera;
        private Button buttonRemoveCam;
    }
}