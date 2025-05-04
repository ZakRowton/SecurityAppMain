namespace Security.Settings
{
    partial class FormSettingsRooms
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
            components = new System.ComponentModel.Container();
            listBoxRooms = new ListBox();
            label1 = new Label();
            label2 = new Label();
            buttonNewRoom = new Button();
            panelCameraSettings = new Panel();
            textBoxCameraName = new TextBox();
            label3 = new Label();
            buttonAddRoom = new Button();
            labelStatus = new Label();
            timerHideLabels = new System.Windows.Forms.Timer(components);
            buttonDeleteRoom = new Button();
            panelCameraSettings.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxRooms
            // 
            listBoxRooms.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxRooms.BackColor = Color.FromArgb(75, 75, 75);
            listBoxRooms.BorderStyle = BorderStyle.FixedSingle;
            listBoxRooms.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listBoxRooms.ForeColor = SystemColors.Window;
            listBoxRooms.FormattingEnabled = true;
            listBoxRooms.ItemHeight = 27;
            listBoxRooms.Location = new Point(40, 88);
            listBoxRooms.Name = "listBoxRooms";
            listBoxRooms.Size = new Size(307, 407);
            listBoxRooms.TabIndex = 0;
            listBoxRooms.SelectedIndexChanged += listBoxRooms_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tempus Sans ITC", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(1082, 9);
            label1.Name = "label1";
            label1.Size = new Size(120, 42);
            label1.TabIndex = 1;
            label1.Text = "Rooms";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(34, 48);
            label2.Name = "label2";
            label2.Size = new Size(79, 27);
            label2.TabIndex = 2;
            label2.Text = "Rooms:";
            // 
            // buttonNewRoom
            // 
            buttonNewRoom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonNewRoom.BackColor = Color.FromArgb(75, 75, 75);
            buttonNewRoom.FlatStyle = FlatStyle.Popup;
            buttonNewRoom.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonNewRoom.ForeColor = SystemColors.ButtonHighlight;
            buttonNewRoom.Location = new Point(40, 529);
            buttonNewRoom.Name = "buttonNewRoom";
            buttonNewRoom.Size = new Size(307, 38);
            buttonNewRoom.TabIndex = 3;
            buttonNewRoom.Text = "NEW ROOM";
            buttonNewRoom.UseVisualStyleBackColor = false;
            buttonNewRoom.Click += buttonNewRoom_Click;
            // 
            // panelCameraSettings
            // 
            panelCameraSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelCameraSettings.Controls.Add(textBoxCameraName);
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
            // buttonAddRoom
            // 
            buttonAddRoom.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonAddRoom.BackColor = Color.FromArgb(75, 75, 75);
            buttonAddRoom.FlatStyle = FlatStyle.Popup;
            buttonAddRoom.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAddRoom.ForeColor = SystemColors.ButtonHighlight;
            buttonAddRoom.Location = new Point(989, 529);
            buttonAddRoom.Name = "buttonAddRoom";
            buttonAddRoom.Size = new Size(191, 38);
            buttonAddRoom.TabIndex = 5;
            buttonAddRoom.Text = "ADD ROOM";
            buttonAddRoom.UseVisualStyleBackColor = false;
            buttonAddRoom.Visible = false;
            buttonAddRoom.Click += buttonAddRoom_Click;
            // 
            // labelStatus
            // 
            labelStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelStatus.AutoSize = true;
            labelStatus.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelStatus.ForeColor = SystemColors.ButtonHighlight;
            labelStatus.Location = new Point(376, 535);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(379, 27);
            labelStatus.TabIndex = 6;
            labelStatus.Text = "Successfully Added New Room To Room!";
            labelStatus.Visible = false;
            // 
            // timerHideLabels
            // 
            timerHideLabels.Interval = 5000;
            timerHideLabels.Tick += timerHideLabels_Tick;
            // 
            // buttonDeleteRoom
            // 
            buttonDeleteRoom.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDeleteRoom.BackColor = Color.FromArgb(75, 75, 75);
            buttonDeleteRoom.FlatStyle = FlatStyle.Popup;
            buttonDeleteRoom.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonDeleteRoom.ForeColor = SystemColors.ButtonHighlight;
            buttonDeleteRoom.Location = new Point(792, 529);
            buttonDeleteRoom.Name = "buttonDeleteRoom";
            buttonDeleteRoom.Size = new Size(191, 38);
            buttonDeleteRoom.TabIndex = 7;
            buttonDeleteRoom.Text = "REMOVE ROOM";
            buttonDeleteRoom.UseVisualStyleBackColor = false;
            buttonDeleteRoom.Visible = false;
            buttonDeleteRoom.Click += buttonDeleteRoom_Click;
            // 
            // FormSettingsRooms
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1214, 586);
            Controls.Add(buttonDeleteRoom);
            Controls.Add(labelStatus);
            Controls.Add(buttonAddRoom);
            Controls.Add(panelCameraSettings);
            Controls.Add(buttonNewRoom);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBoxRooms);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormSettingsRooms";
            Text = "FormSettingsCameras";
            Load += FormSettingsRooms_Load;
            panelCameraSettings.ResumeLayout(false);
            panelCameraSettings.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBoxRooms;
        private Label label1;
        private Label label2;
        private Button buttonNewRoom;
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
        private Button buttonAddRoom;
        private Label labelStatus;
        private System.Windows.Forms.Timer timerHideLabels;
        private Button buttonDeleteRoom;
    }
}