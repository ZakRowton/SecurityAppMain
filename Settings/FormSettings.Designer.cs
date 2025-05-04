namespace Security.Settings
{
    partial class FormSettings
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
            panel1 = new Panel();
            panelSettings = new Panel();
            buttonCams = new Button();
            buttonSensorsNav = new Button();
            buttonRoomsNav = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panel1.BackColor = Color.FromArgb(50, 50, 50);
            panel1.Controls.Add(buttonRoomsNav);
            panel1.Controls.Add(buttonSensorsNav);
            panel1.Controls.Add(buttonCams);
            panel1.Location = new Point(10, 10);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(230, 652);
            panel1.TabIndex = 0;
            // 
            // panelSettings
            // 
            panelSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelSettings.Location = new Point(250, 12);
            panelSettings.Margin = new Padding(3, 2, 3, 2);
            panelSettings.Name = "panelSettings";
            panelSettings.Size = new Size(1135, 649);
            panelSettings.TabIndex = 1;
            // 
            // buttonCams
            // 
            buttonCams.BackColor = Color.Transparent;
            buttonCams.FlatStyle = FlatStyle.Flat;
            buttonCams.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCams.ForeColor = SystemColors.ButtonHighlight;
            buttonCams.Location = new Point(10, 96);
            buttonCams.Name = "buttonCams";
            buttonCams.Size = new Size(210, 38);
            buttonCams.TabIndex = 9;
            buttonCams.Text = "CAMERAS";
            buttonCams.UseVisualStyleBackColor = false;
            buttonCams.Click += btnNavCameras_Click;
            // 
            // buttonSensorsNav
            // 
            buttonSensorsNav.BackColor = Color.Transparent;
            buttonSensorsNav.FlatStyle = FlatStyle.Flat;
            buttonSensorsNav.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonSensorsNav.ForeColor = SystemColors.ButtonHighlight;
            buttonSensorsNav.Location = new Point(10, 152);
            buttonSensorsNav.Name = "buttonSensorsNav";
            buttonSensorsNav.Size = new Size(210, 38);
            buttonSensorsNav.TabIndex = 10;
            buttonSensorsNav.Text = "SENSORS";
            buttonSensorsNav.UseVisualStyleBackColor = false;
            buttonSensorsNav.Click += buttonSensors_Click;
            // 
            // buttonRoomsNav
            // 
            buttonRoomsNav.BackColor = Color.Transparent;
            buttonRoomsNav.FlatStyle = FlatStyle.Flat;
            buttonRoomsNav.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonRoomsNav.ForeColor = SystemColors.ButtonHighlight;
            buttonRoomsNav.Location = new Point(10, 14);
            buttonRoomsNav.Name = "buttonRoomsNav";
            buttonRoomsNav.Size = new Size(210, 38);
            buttonRoomsNav.TabIndex = 11;
            buttonRoomsNav.Text = "ROOMS";
            buttonRoomsNav.UseVisualStyleBackColor = false;
            buttonRoomsNav.Click += buttonRooms_Click;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1394, 682);
            Controls.Add(panelSettings);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormSettings";
            Text = "FormSettings";
            Load += FormSettings_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panelSettings;
        private Button buttonRoomsNav;
        private Button buttonSensorsNav;
        private Button buttonCams;
    }
}