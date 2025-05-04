namespace Security
{
    partial class FormBlueprintDesigner
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
            pbSave = new PictureBox();
            panel1 = new Panel();
            comboBoxSensors = new ComboBox();
            buttonNewSensor = new Button();
            comboBox1 = new ComboBox();
            buttonAddCam = new Button();
            ((System.ComponentModel.ISupportInitialize)pbSave).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pbSave
            // 
            pbSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbSave.Image = Properties.Resources.save;
            pbSave.Location = new Point(1147, 6);
            pbSave.Name = "pbSave";
            pbSave.Size = new Size(69, 53);
            pbSave.SizeMode = PictureBoxSizeMode.StretchImage;
            pbSave.TabIndex = 0;
            pbSave.TabStop = false;
            pbSave.Click += pbSave_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.FromArgb(55, 55, 55);
            panel1.Controls.Add(comboBoxSensors);
            panel1.Controls.Add(pbSave);
            panel1.Controls.Add(buttonNewSensor);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(buttonAddCam);
            panel1.Location = new Point(9, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(1219, 64);
            panel1.TabIndex = 1;
            // 
            // comboBoxSensors
            // 
            comboBoxSensors.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBoxSensors.FormattingEnabled = true;
            comboBoxSensors.Location = new Point(400, 16);
            comboBoxSensors.Name = "comboBoxSensors";
            comboBoxSensors.Size = new Size(251, 29);
            comboBoxSensors.TabIndex = 3;
            // 
            // buttonNewSensor
            // 
            buttonNewSensor.Location = new Point(657, 16);
            buttonNewSensor.Name = "buttonNewSensor";
            buttonNewSensor.Size = new Size(97, 30);
            buttonNewSensor.TabIndex = 2;
            buttonNewSensor.Text = "New Sensor";
            buttonNewSensor.UseVisualStyleBackColor = true;
            buttonNewSensor.Click += buttonNewSensor_Click;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(24, 16);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(251, 29);
            comboBox1.TabIndex = 1;
            // 
            // buttonAddCam
            // 
            buttonAddCam.Location = new Point(281, 16);
            buttonAddCam.Name = "buttonAddCam";
            buttonAddCam.Size = new Size(97, 30);
            buttonAddCam.TabIndex = 0;
            buttonAddCam.Text = "New Camera";
            buttonAddCam.UseVisualStyleBackColor = true;
            buttonAddCam.Click += buttonAddCam_Click;
            // 
            // FormBlueprintDesigner
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(64, 64, 64);
            BackgroundImage = Properties.Resources.blueprint2;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1237, 679);
            Controls.Add(panel1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormBlueprintDesigner";
            Text = "FormBlueprintDesigner";
            Load += FormBlueprintDesigner_Load;
            ((System.ComponentModel.ISupportInitialize)pbSave).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbSave;
        private Panel panel1;
        private Button buttonAddCam;
        private ComboBox comboBox1;
        private ComboBox comboBoxSensors;
        private Button buttonNewSensor;
    }
}