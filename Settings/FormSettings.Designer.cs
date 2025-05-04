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
            btnNavCameras = new Button();
            buttonSensors = new Button();
            panelSettings = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panel1.BackColor = Color.FromArgb(50, 50, 50);
            panel1.Controls.Add(buttonSensors);
            panel1.Controls.Add(btnNavCameras);
            panel1.Location = new Point(12, 14);
            panel1.Name = "panel1";
            panel1.Size = new Size(263, 870);
            panel1.TabIndex = 0;
            // 
            // btnNavCameras
            // 
            btnNavCameras.Font = new Font("Segoe UI Historic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNavCameras.Location = new Point(11, 22);
            btnNavCameras.Name = "btnNavCameras";
            btnNavCameras.Size = new Size(240, 51);
            btnNavCameras.TabIndex = 0;
            btnNavCameras.Text = "Cameras";
            btnNavCameras.UseVisualStyleBackColor = true;
            // 
            // buttonSensors
            // 
            buttonSensors.Font = new Font("Segoe UI Historic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonSensors.Location = new Point(11, 106);
            buttonSensors.Name = "buttonSensors";
            buttonSensors.Size = new Size(240, 51);
            buttonSensors.TabIndex = 1;
            buttonSensors.Text = "Sensors";
            buttonSensors.UseVisualStyleBackColor = true;
            // 
            // panelSettings
            // 
            panelSettings.Location = new Point(286, 16);
            panelSettings.Name = "panelSettings";
            panelSettings.Size = new Size(1297, 865);
            panelSettings.TabIndex = 1;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1593, 909);
            Controls.Add(panelSettings);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormSettings";
            Text = "FormSettings";
            Load += FormSettings_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnNavCameras;
        private Button buttonSensors;
        private Panel panelSettings;
    }
}