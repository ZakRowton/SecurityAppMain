using Vlc.DotNet.Forms;

namespace Security
{
    partial class FormBlueprintView
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
            panelCam = new Panel();
            pbExit = new PictureBox();
            vlcControl1 = new VlcControl();
            panelSmartThings = new Panel();
            buttonFridgeTempUp = new Button();
            buttonFridgeTempDown = new Button();
            buttonFreezerTempUp = new Button();
            buttonFreezerTempDown = new Button();
            labelFreezerTemp = new Label();
            label3 = new Label();
            labelFridgeTemp = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            pbSmartThings = new PictureBox();
            labelStatusMessage = new Label();
            panelCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbExit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)vlcControl1).BeginInit();
            panelSmartThings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbSmartThings).BeginInit();
            SuspendLayout();
            // 
            // panelCam
            // 
            panelCam.Controls.Add(pbExit);
            panelCam.Controls.Add(vlcControl1);
            panelCam.Location = new Point(127, 66);
            panelCam.Name = "panelCam";
            panelCam.Size = new Size(1025, 529);
            panelCam.TabIndex = 0;
            panelCam.Visible = false;
            // 
            // pbExit
            // 
            pbExit.Image = Properties.Resources.exit;
            pbExit.Location = new Point(959, 15);
            pbExit.Name = "pbExit";
            pbExit.Size = new Size(52, 47);
            pbExit.SizeMode = PictureBoxSizeMode.StretchImage;
            pbExit.TabIndex = 1;
            pbExit.TabStop = false;
            pbExit.Click += pbExit_Click;
            // 
            // vlcControl1
            // 
            vlcControl1.BackColor = Color.Black;
            vlcControl1.Dock = DockStyle.Fill;
            vlcControl1.Location = new Point(0, 0);
            vlcControl1.Name = "vlcControl1";
            vlcControl1.Size = new Size(1025, 529);
            vlcControl1.Spu = -1;
            vlcControl1.TabIndex = 0;
            vlcControl1.Text = "vlcControl1";
            vlcControl1.VlcLibDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "libvlc", (IntPtr.Size == 4 ? "win-x86" : "win-x64")));
            vlcControl1.VlcMediaplayerOptions = null;
            // 
            // panelSmartThings
            // 
            panelSmartThings.Controls.Add(labelStatusMessage);
            panelSmartThings.Controls.Add(buttonFridgeTempUp);
            panelSmartThings.Controls.Add(buttonFridgeTempDown);
            panelSmartThings.Controls.Add(buttonFreezerTempUp);
            panelSmartThings.Controls.Add(buttonFreezerTempDown);
            panelSmartThings.Controls.Add(labelFreezerTemp);
            panelSmartThings.Controls.Add(label3);
            panelSmartThings.Controls.Add(labelFridgeTemp);
            panelSmartThings.Controls.Add(label1);
            panelSmartThings.Controls.Add(pictureBox1);
            panelSmartThings.Location = new Point(127, 65);
            panelSmartThings.Name = "panelSmartThings";
            panelSmartThings.Size = new Size(1025, 529);
            panelSmartThings.TabIndex = 2;
            panelSmartThings.Visible = false;
            // 
            // buttonFridgeTempUp
            // 
            buttonFridgeTempUp.Location = new Point(339, 44);
            buttonFridgeTempUp.Name = "buttonFridgeTempUp";
            buttonFridgeTempUp.Size = new Size(30, 28);
            buttonFridgeTempUp.TabIndex = 9;
            buttonFridgeTempUp.Text = "+";
            buttonFridgeTempUp.UseVisualStyleBackColor = true;
            buttonFridgeTempUp.Click += buttonFridgeTempUp_Click;
            // 
            // buttonFridgeTempDown
            // 
            buttonFridgeTempDown.Location = new Point(237, 44);
            buttonFridgeTempDown.Name = "buttonFridgeTempDown";
            buttonFridgeTempDown.Size = new Size(30, 28);
            buttonFridgeTempDown.TabIndex = 8;
            buttonFridgeTempDown.Text = "-";
            buttonFridgeTempDown.UseVisualStyleBackColor = true;
            buttonFridgeTempDown.Click += buttonFridgeTempDown_Click;
            // 
            // buttonFreezerTempUp
            // 
            buttonFreezerTempUp.Location = new Point(339, 85);
            buttonFreezerTempUp.Name = "buttonFreezerTempUp";
            buttonFreezerTempUp.Size = new Size(30, 28);
            buttonFreezerTempUp.TabIndex = 7;
            buttonFreezerTempUp.Text = "+";
            buttonFreezerTempUp.UseVisualStyleBackColor = true;
            buttonFreezerTempUp.Click += buttonFreezerTempUp_Click;
            // 
            // buttonFreezerTempDown
            // 
            buttonFreezerTempDown.Location = new Point(237, 85);
            buttonFreezerTempDown.Name = "buttonFreezerTempDown";
            buttonFreezerTempDown.Size = new Size(30, 28);
            buttonFreezerTempDown.TabIndex = 6;
            buttonFreezerTempDown.Text = "-";
            buttonFreezerTempDown.UseVisualStyleBackColor = true;
            buttonFreezerTempDown.Click += buttonFreezerTempDown_Click;
            // 
            // labelFreezerTemp
            // 
            labelFreezerTemp.AutoSize = true;
            labelFreezerTemp.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFreezerTemp.ForeColor = SystemColors.ButtonHighlight;
            labelFreezerTemp.Location = new Point(287, 88);
            labelFreezerTemp.Name = "labelFreezerTemp";
            labelFreezerTemp.Size = new Size(33, 25);
            labelFreezerTemp.TabIndex = 5;
            labelFreezerTemp.Text = "0F";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ButtonHighlight;
            label3.Location = new Point(54, 85);
            label3.Name = "label3";
            label3.Size = new Size(135, 25);
            label3.TabIndex = 4;
            label3.Text = "Freezer Temp:";
            // 
            // labelFridgeTemp
            // 
            labelFridgeTemp.AutoSize = true;
            labelFridgeTemp.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFridgeTemp.ForeColor = SystemColors.ButtonHighlight;
            labelFridgeTemp.Location = new Point(287, 47);
            labelFridgeTemp.Name = "labelFridgeTemp";
            labelFridgeTemp.Size = new Size(33, 25);
            labelFridgeTemp.TabIndex = 3;
            labelFridgeTemp.Text = "0F";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(54, 47);
            label1.Name = "label1";
            label1.Size = new Size(127, 25);
            label1.TabIndex = 2;
            label1.Text = "Fridge Temp:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.exit;
            pictureBox1.Location = new Point(973, 15);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(38, 37);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // pbSmartThings
            // 
            pbSmartThings.Image = Properties.Resources.SmartThings;
            pbSmartThings.Location = new Point(12, 615);
            pbSmartThings.Name = "pbSmartThings";
            pbSmartThings.Size = new Size(54, 52);
            pbSmartThings.SizeMode = PictureBoxSizeMode.StretchImage;
            pbSmartThings.TabIndex = 2;
            pbSmartThings.TabStop = false;
            pbSmartThings.Click += pbSmartThings_Click;
            // 
            // labelStatusMessage
            // 
            labelStatusMessage.AutoSize = true;
            labelStatusMessage.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelStatusMessage.ForeColor = SystemColors.ButtonHighlight;
            labelStatusMessage.Location = new Point(54, 484);
            labelStatusMessage.Name = "labelStatusMessage";
            labelStatusMessage.Size = new Size(135, 25);
            labelStatusMessage.TabIndex = 10;
            labelStatusMessage.Text = "Freezer Temp:";
            // 
            // FormBlueprintView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(65, 65, 65);
            ClientSize = new Size(1253, 679);
            Controls.Add(pbSmartThings);
            Controls.Add(panelSmartThings);
            Controls.Add(panelCam);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormBlueprintView";
            Text = "FormBlueprintView";
            Load += FormBlueprintView_Load;
            panelCam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbExit).EndInit();
            ((System.ComponentModel.ISupportInitialize)vlcControl1).EndInit();
            panelSmartThings.ResumeLayout(false);
            panelSmartThings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbSmartThings).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelCam;
        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private PictureBox pbExit;
        private Panel panelSmartThings;
        private PictureBox pictureBox1;
        private PictureBox pbSmartThings;
        private Label labelFreezerTemp;
        private Label label3;
        private Label labelFridgeTemp;
        private Label label1;
        private Button buttonFridgeTempUp;
        private Button buttonFridgeTempDown;
        private Button buttonFreezerTempUp;
        private Button buttonFreezerTempDown;
        private Label labelStatusMessage;
    }
}