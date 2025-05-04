using Security.Blueprints;
using Security.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Security
{

    public partial class FormBlueprintDesigner : Form
    {
        private BlueprintDesign Design { get; set; } = null;
        private Storage Storage { get; set; } = null;

        // --- Fields for Dragging ---
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Control currentDragControl = null;
        // --- End Fields for Dragging ---

        public FormBlueprintDesigner(BlueprintDesign blueprintDesign, Storage storage)
        {
            Design = blueprintDesign ?? new BlueprintDesign(); // Ensure Design is not null
            Storage = storage ?? throw new ArgumentNullException(nameof(storage)); // Storage is required

            InitializeComponent(); // Initialize controls FIRST

            // Load background image AFTER InitializeComponent
            try
            {
                if (!string.IsNullOrEmpty(Design.BackgroundImage) && System.IO.File.Exists(Design.BackgroundImage))
                {
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                    this.BackgroundImage = Image.FromFile(Design.BackgroundImage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading background image '{Design.BackgroundImage}': {ex.Message}", "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CORRECTED METHOD ---
        private void LoadCurrentDesignControls()
        {
            // Ensure the list exists and Design is not null
            if (Design?.CameraControls == null)
            {
                Console.WriteLine("Design or CameraControls list is null. Cannot load controls.");
                return;
            }

            Console.WriteLine($"Loading {Design.CameraControls.Count} controls from design...");

            foreach (CameraControl control in Design.CameraControls)
            {
                if (control == null) // Check if the entry itself is null
                {
                    Console.WriteLine("Null CameraControl entry found in design. Skipping.");
                    continue;
                }

                // Get the associated Camera data - essential for Tag and maybe Name
                Camera camData = control.Camera;
                if (camData == null)
                {
                    // Attempt to find based on name if Camera reference is missing? (Optional fallback)
                    // camData = Storage.Cameras.FirstOrDefault(c => c.Name == control.Name);
                    // if (camData == null)
                    //{
                    Console.WriteLine($"Warning: CameraControl '{control.Name ?? "Unnamed"}' has no associated Camera data. Skipping.");
                    continue; // Skip if we can't link to a Camera object
                    //}
                }

                PictureBox pbNewCam = new PictureBox();
                try
                {
                    // Consider making the resource name configurable if needed
                    pbNewCam.Image = Properties.Resources.camera;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading camera image resource: {ex.Message}", "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Continue without image? Or skip this control?
                }

                // Use Camera's name for consistency, ensure uniqueness if needed
                pbNewCam.Name = "buttonCamera" + camData.Name?.Replace(" ", "") ?? Guid.NewGuid().ToString(); // Use GUID if name is bad
                pbNewCam.Tag = camData; // Store the actual CAMERA object in the Tag
                pbNewCam.Size = new Size(69, 53);
                pbNewCam.SizeMode = PictureBoxSizeMode.StretchImage;
                pbNewCam.BackColor = Color.Transparent;

                // --- FIX: Use the saved position directly ---
                // Use the Position property from the CameraControl object
                pbNewCam.Location = control.Position;
                // Optional: Add boundary check here too, in case saved position is outside current form bounds
                AdjustLocationToBounds(pbNewCam);
                // --- End FIX ---

                // Attach drag handlers
                pbNewCam.MouseDown += PbCam_MouseDown;
                pbNewCam.MouseMove += PbCam_MouseMove;
                pbNewCam.MouseUp += PbCam_MouseUp;
                pbNewCam.Cursor = Cursors.Hand;

                // Add the Control to the Form's controls collection
                this.Controls.Add(pbNewCam);
                pbNewCam.BringToFront();
                Console.WriteLine($"Loaded control '{pbNewCam.Name}' at {pbNewCam.Location}");
            }
        }

        // Helper method to adjust location if it's outside bounds (used in Load and Move)
        private void AdjustLocationToBounds(Control ctrl)
        {
            if (ctrl == null) return;

            int newX = ctrl.Location.X;
            int newY = ctrl.Location.Y;

            if (newX < 0) newX = 0;
            if (newY < 0) newY = 0;
            if (newX + ctrl.Width > this.Size.Width)
            {
                newX = this.Size.Width - ctrl.Width;
            }
            if (newY + ctrl.Height > this.Size.Height)
            {
                newY = this.Size.Height - ctrl.Height;
            }

            // Only update if changed
            if (ctrl.Location.X != newX || ctrl.Location.Y != newY)
            {
                ctrl.Location = new Point(newX, newY);
            }
        }


        private void buttonNewSensor_Click(object sender, EventArgs e)
        {
            // 1. Get selected camera name
            string selectedSensorName = comboBoxSensors.Text;
            if (string.IsNullOrWhiteSpace(selectedSensorName))
            {
                MessageBox.Show("Please select a sensor from the list.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Find the corresponding Camera object
            DoorSensor sensorToUse = null;
            if (Storage.DoorSensors != null)
            {
                sensorToUse = Storage.DoorSensors.FirstOrDefault(sensor => sensor != null && sensor.SensorName == selectedSensorName);
            }

            // 3. Check if the camera was found
            if (sensorToUse == null)
            {
                MessageBox.Show($"Could not find sensor data for '{selectedSensorName}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Create and Configure the PictureBox
            PictureBox pbNewSensor = new PictureBox();
            try
            {
                pbNewSensor.Image = Properties.Resources.door_closed1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading camera image resource: {ex.Message}", "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            pbNewSensor.Tag = sensorToUse; // Tag holds the CAMERA object
            pbNewSensor.Size = new Size(69, 53);
            pbNewSensor.SizeMode = PictureBoxSizeMode.StretchImage;
            pbNewSensor.BackColor = Color.Transparent;

            // --- Dynamic Location for NEW cameras (simple stacking) ---
            int verticalOffset = 10;
            int horizontalOffset = 10;
            int spacing = 5;
            int existingCamControlsCount = this.Controls.OfType<PictureBox>().Count(pb => pb.Tag is Camera); // Count PictureBoxes with Camera Tag
            int newY = verticalOffset + (existingCamControlsCount * (pbNewSensor.Height + spacing));
            int newX = horizontalOffset;

            // Basic wrap-around if stacking goes off bottom
            int maxPerRow = (this.ClientSize.Width - horizontalOffset * 2) / (pbNewSensor.Width + spacing);
            if (maxPerRow <= 0) maxPerRow = 1;
            newX = horizontalOffset + (existingCamControlsCount % maxPerRow) * (pbNewSensor.Width + spacing);
            newY = verticalOffset + (existingCamControlsCount / maxPerRow) * (pbNewSensor.Height + spacing);

            pbNewSensor.Location = new Point(newX, newY);
            //AdjustLocationToBounds(pbNewCam); // Ensure it's within bounds
            // --------------------------------------------------------------------

            pbNewSensor.Name = "buttonSensor" + sensorToUse.SensorName?.Replace(" ", "") ?? Guid.NewGuid().ToString();

            // Attach Drag Handlers and Set Cursor
            pbNewSensor.MouseDown += PbSensor_MouseDown;
            pbNewSensor.MouseMove += PbSensor_MouseMove;
            pbNewSensor.MouseUp += PbSensor_MouseUp;
            pbNewSensor.Cursor = Cursors.Hand;

            // 5. Add the Control to the Form
            Controls.Add(pbNewSensor);
            pbNewSensor.BringToFront();
            this.Refresh();
        }


        private void buttonAddCam_Click(object sender, EventArgs e)
        {
            // 1. Get selected camera name
            string selectedCameraName = comboBox1.Text;
            if (string.IsNullOrWhiteSpace(selectedCameraName))
            {
                MessageBox.Show("Please select a camera from the list.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Find the corresponding Camera object
            Camera camToUse = null;
            if (Storage.Cameras != null)
            {
                camToUse = Storage.Cameras.FirstOrDefault(camera => camera != null && camera.Name == selectedCameraName);
            }

            // 3. Check if the camera was found
            if (camToUse == null)
            {
                MessageBox.Show($"Could not find camera data for '{selectedCameraName}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Create and Configure the PictureBox
            PictureBox pbNewCam = new PictureBox();
            try
            {
                pbNewCam.Image = Properties.Resources.camera;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading camera image resource: {ex.Message}", "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            pbNewCam.Tag = camToUse; // Tag holds the CAMERA object
            pbNewCam.Size = new Size(69, 53);
            pbNewCam.SizeMode = PictureBoxSizeMode.StretchImage;
            pbNewCam.BackColor = Color.Transparent;

            // --- Dynamic Location for NEW cameras (simple stacking) ---
            int verticalOffset = 10;
            int horizontalOffset = 10;
            int spacing = 5;
            int existingCamControlsCount = this.Controls.OfType<PictureBox>().Count(pb => pb.Tag is Camera); // Count PictureBoxes with Camera Tag
            int newY = verticalOffset + (existingCamControlsCount * (pbNewCam.Height + spacing));
            int newX = horizontalOffset;

            // Basic wrap-around if stacking goes off bottom
            int maxPerRow = (this.ClientSize.Width - horizontalOffset * 2) / (pbNewCam.Width + spacing);
            if (maxPerRow <= 0) maxPerRow = 1;
            newX = horizontalOffset + (existingCamControlsCount % maxPerRow) * (pbNewCam.Width + spacing);
            newY = verticalOffset + (existingCamControlsCount / maxPerRow) * (pbNewCam.Height + spacing);

            pbNewCam.Location = new Point(newX, newY);
            //AdjustLocationToBounds(pbNewCam); // Ensure it's within bounds
            // --------------------------------------------------------------------

            pbNewCam.Name = "buttonCamera" + camToUse.Name?.Replace(" ", "") ?? Guid.NewGuid().ToString();

            // Attach Drag Handlers and Set Cursor
            pbNewCam.MouseDown += PbCam_MouseDown;
            pbNewCam.MouseMove += PbCam_MouseMove;
            pbNewCam.MouseUp += PbCam_MouseUp;
            pbNewCam.Cursor = Cursors.Hand;

            // 5. Add the Control to the Form
            Controls.Add(pbNewCam);
            pbNewCam.BringToFront();
            this.Refresh();
        }

        private List<DoorSensor> AvailableDevices { get; set; } = new List<DoorSensor>();
        private async void ScanForSensors()
        {
            SensorScanner Scanner = new SensorScanner();

            Console.WriteLine("Starting network scan...");
            // Define the range: 192.168.0.100 to 192.168.0.110
            string baseIp = "192.168.0";
            int start = 100;
            int end = 110;
            int scanTimeout = 1000; // milliseconds - adjust as needed

            try
            {
                int sensorCt = 1;
                List<IPAddress> onlineDevices = await Scanner.FindDevicesAsync(baseIp, start, end, scanTimeout);

                foreach (IPAddress onlineDevice in onlineDevices)
                {
                    DoorSensor availableSensor = new DoorSensor();
                    availableSensor.SensorName = "Available Sensor " + sensorCt;
                    availableSensor.IPAddress = onlineDevice.ToString();
                    availableSensor.RoomID = "NONE";
                    comboBoxSensors.Items.Add(availableSensor.SensorName);
                    AvailableDevices.Add(availableSensor);
                    sensorCt++;
                }

                sensorCt = 1;
                if (comboBoxSensors.Items.Count > 0) comboBoxSensors.SelectedIndex = 0;
                Console.WriteLine("Scan complete.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAn error occurred during the scan: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.ResetColor();
            }
        }

        private void FormBlueprintDesigner_Load(object sender, EventArgs e)
        {
            ScanForSensors();

            // Populate ComboBox
            if (Storage?.Cameras != null)
            {
                foreach (Camera camera in Storage.Cameras)
                {
                    if (camera != null && !string.IsNullOrEmpty(camera.Name))
                    {
                        comboBox1.Items.Add(camera.Name);
                    }
                }
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("Camera storage is not available.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Load controls AFTER ComboBox is populated but before form is shown
            LoadCurrentDesignControls();
        }

        #region Camera Drag/Drop

        // --- Drag and Drop Event Handlers (Remain the same) ---
        private void PbCam_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sender is Control control)
            {
                isDragging = true;
                currentDragControl = control;
                dragStartPoint = e.Location;
                control.Cursor = Cursors.SizeAll;
                control.BringToFront();
            }
        }

        private void PbCam_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && currentDragControl != null)
            {
                Point currentMousePos = this.PointToClient(Control.MousePosition);
                int newX = currentMousePos.X - dragStartPoint.X;
                int newY = currentMousePos.Y - dragStartPoint.Y;

                // Use helper method for boundary check
                Point proposedLocation = new Point(newX, newY);
                currentDragControl.Location = proposedLocation; // Set temporarily
                AdjustLocationToBounds(currentDragControl); // Adjust if needed
            }
        }

        private void PbCam_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDragging)
            {
                if (currentDragControl != null)
                {
                    currentDragControl.Cursor = Cursors.Hand;
                    // --- Update Position in the underlying CameraControl Object ---
                    // This makes the change persistent in the Design object *before* saving
                    if (currentDragControl.Tag is Camera cam)
                    {
                        // Find the CameraControl in the Design list that corresponds to this Camera
                        CameraControl existingCtrl = Design.CameraControls.FirstOrDefault(cc => cc.Camera == cam);
                        if (existingCtrl != null)
                        {
                            existingCtrl.Position = currentDragControl.Location; // Update position in the Design object
                            Console.WriteLine($"Updated Design: Camera '{cam.Name}' moved to {existingCtrl.Position}");
                        }
                        else
                        {
                            // This scenario (dragged control not having corresponding CameraControl)
                            // shouldn't happen if loading/adding is correct, but good to note.
                            Console.WriteLine($"Warning: Could not find CameraControl in Design for Camera '{cam.Name}' during MouseUp.");
                        }
                    }
                    // --- End Update ---
                }
                isDragging = false;
                currentDragControl = null;
                dragStartPoint = Point.Empty;
            }
        }
        // --- End Drag and Drop Event Handlers ---
        #endregion

        #region Sensor Drag/Drop


        // --- Drag and Drop Event Handlers (Remain the same) ---
        private void PbSensor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sender is Control control)
            {
                isDragging = true;
                currentDragControl = control;
                dragStartPoint = e.Location;
                control.Cursor = Cursors.SizeAll;
                control.BringToFront();
            }
        }

        private void PbSensor_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && currentDragControl != null)
            {
                Point currentMousePos = this.PointToClient(Control.MousePosition);
                int newX = currentMousePos.X - dragStartPoint.X;
                int newY = currentMousePos.Y - dragStartPoint.Y;

                // Use helper method for boundary check
                Point proposedLocation = new Point(newX, newY);
                currentDragControl.Location = proposedLocation; // Set temporarily
                AdjustLocationToBounds(currentDragControl); // Adjust if needed
            }
        }

        private void PbSensor_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDragging)
            {
                if (currentDragControl != null)
                {
                    currentDragControl.Cursor = Cursors.Hand;
                    // --- Update Position in the underlying CameraControl Object ---
                    // This makes the change persistent in the Design object *before* saving
                    if (currentDragControl.Tag is DoorSensor cam)
                    {
                        // Find the CameraControl in the Design list that corresponds to this Camera
                        SensorControl existingCtrl = Design.SensorControls.FirstOrDefault(cc => cc.Sensor == cam);
                        if (existingCtrl != null)
                        {
                            existingCtrl.Position = currentDragControl.Location; // Update position in the Design object
                            Console.WriteLine($"Updated Design: Camera '{cam.SensorName}' moved to {existingCtrl.Position}");
                        }
                        else
                        {
                            // This scenario (dragged control not having corresponding CameraControl)
                            // shouldn't happen if loading/adding is correct, but good to note.
                            Console.WriteLine($"Warning: Could not find CameraControl in Design for Camera '{cam.SensorName}' during MouseUp.");
                        }
                    }
                    // --- End Update ---
                }
                isDragging = false;
                currentDragControl = null;
                dragStartPoint = Point.Empty;
            }
        }
        #endregion


        // --- ADJUSTED SAVE METHOD ---
        private void pbSave_Click(object sender, EventArgs e)
        {
            Design.CameraControls.Clear();

            foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
            {
                // Cameras
                if (pb.Tag is Camera tag) // Check if the tag holds a Camera object
                {
                    // Create a NEW CameraControl entry reflecting the UI state
                    CameraControl camCtrl = new CameraControl();
                    camCtrl.Name = tag.Name; // Use the Camera's name
                    camCtrl.Camera = (Camera)pb.Tag; // Link to the Camera object
                    camCtrl.Position = pb.Location; // Get the CURRENT location from the PictureBox

                    Design.CameraControls.Add(camCtrl); // Add the current state to the list
                }

                // Door Sensors
                if (pb.Tag is DoorSensor tagSensor) // Check if the tag holds a Camera object
                {
                    // Create a NEW CameraControl entry reflecting the UI state
                    SensorControl sensorCtrl = new SensorControl();
                    sensorCtrl.Name = tagSensor.SensorName; // Use the Camera's name
                    sensorCtrl.Sensor = (DoorSensor)pb.Tag; // Link to the Camera object
                    sensorCtrl.Position = pb.Location; // Get the CURRENT location from the PictureBox

                    Design.SensorControls.Add(sensorCtrl); // Add the current state to the list
                }
            }

            try
            {
                Design.Save(); // Call your save implementation
                MessageBox.Show("Blueprint saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving blueprint: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}