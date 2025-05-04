using System; // Added for EventArgs, Exception, etc.
using System.Diagnostics;
using System.Drawing; // Added for Color
using System.Windows.Forms;
using System.Text.Json; // Keep if used elsewhere
using System.Timers;   // <--- Added for System.Timers.Timer
using System.Threading.Tasks;
using static Security.SmartThings.SmartThingsApiClient;
using Security.Sensors;
using Security.Blueprints;
using Security.SmartThings;
using System.Net;
using Security.Settings; // <--- Added for Task
// using static Security.SmartThingsApiClient; // Keep if used elsewhere (Seems unused in this snippet)

namespace Security
{
    public partial class Form1 : Form
    {
        #region Variables

        // --- UI State ---
        private bool isDragging = false;
        private Point dragOffset;     // Keep if used elsewhere
        private Form activeChildForm = null;

        // --- Sub Forms ---
        private Form FormBlueprintView = null;
        private Form FormBlueprintDesigner = null;
        private FormSettings FormSettings = null;

        // --- Sensor Monitoring (Correct Implementation) ---
        private Esp32Manager _espManager;                     // To hold the manager instance
        private System.Timers.Timer _sensorMonitorTimer;      // Timer for background polling
        private const string SensorDeviceId = "Sensor_LivingRoom"; // ID of the sensor to monitor
        private const string SensorDeviceId2 = "Sensor_FrontDoor"; // ID of the sensor to monitor
        private const double SensorMonitorIntervalMs = 5000;  // Check every 5 seconds (adjust as needed)
                                                              // -------------------------------------------------

        // --- PanelNav Size Animation ---#region Variables
        // --- PanelNav Size Animation ---
        private System.Windows.Forms.Timer panelAnimationTimer;
        private Stopwatch panelAnimationStopwatch = new Stopwatch();
        private const int panelStartWidth = 0;          // Collapsed width is 0
        private int panelTargetWidth = 250;             // <<< SET YOUR DESIRED EXPANDED WIDTH HERE
        private TimeSpan panelAnimationDuration = TimeSpan.FromMilliseconds(350); // Animation speed
        private bool isPanelExpanded = false;           // Track panel state
        private int formBaseWidth = -1;                 // Stores the Form's width when panel is collapsed
        private int pictureBoxStartX = -1;
        #endregion

        #region Public Properties
        public Storage Storage { get; set; } = null;
        public BlueprintDesign BlueprintDesign { get; set; } = null;
        #endregion

        #region Constructor
        public Form1()
        {
            // panelTargetSize = new Size(180, Height * 5); // REMOVE THIS LINE

            // panelTargetWidth should be set in the Variables section above.

            Storage = Storage.Load();
            BlueprintDesign = BlueprintDesign.Load();
            InitializeComponent(); // Creates controls from designer (Applies Docking)
            InitializeSubForms();

            InitializePanelAnimationTimer();

            // Set initial panel state AFTER InitializeComponent
            if (panelNav != null)
            {
                panelNav.Width = panelStartWidth; // Start collapsed (width = 0)
                                                  // Height is now handled by Docking property set in the designer!
            }
            else
            {
                Console.WriteLine("Warning: panelNav control not found during initialization.");
            }

            // Hook up FormClosing event for cleanup
            this.FormClosing += Form1_FormClosing;

            // Hook up Resize event
            this.Resize += Form1_Resize; // <<< ADD THIS LINE

            // Hook up animation trigger (assuming pictureBox1)
            if (pictureBox1 != null)
            {
                pictureBox1.Click += pictureBox1_Click;
            }
        }

        private void InitializeSubForms()
        {
            // Consider lazy loading these if they aren't needed immediately
            FormBlueprintView = new FormBlueprintView(Storage, BlueprintDesign);
            FormBlueprintDesigner = new FormBlueprintDesigner(BlueprintDesign, Storage);
            FormSettings = new FormSettings();
        }
        #endregion


        #region Navigation Panel Animation
        // (Keep the InitializePanelAnimationTimer, pictureBox1_Click, PanelAnimationTimer_Tick methods
        //  from the previous "animation fix" answer here)

        private void InitializePanelAnimationTimer()
        {
            panelAnimationTimer?.Dispose(); // Defensive disposal
            panelAnimationTimer = new System.Windows.Forms.Timer(this.components ?? (this.components = new System.ComponentModel.Container()));
            panelAnimationTimer.Interval = 15; // ~66 FPS
            panelAnimationTimer.Tick += PanelAnimationTimer_Tick;
            Console.WriteLine("Panel Animation Timer Initialized.");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("pictureBox1 Clicked.");
            if (panelNav == null || panelAnimationTimer.Enabled || pictureBox1 == null || pictureBoxStartX < 0) // Check pictureBox1 init too
            {
                Console.WriteLine($"Animation trigger ignored: Panel null({panelNav == null}), Timer Enabled({panelAnimationTimer?.Enabled}), PicBox not ready({pictureBox1 == null || pictureBoxStartX < 0})");
                return;
            }

            // Calculate Panel Width Change dynamically
            int panelWidthChange = panelTargetWidth - panelStartWidth;

            // Base width is the width when the panel IS collapsed.
            // If currently expanded, calculate what the base width *was*.
            // If currently collapsed, the current width *is* the base width.
            if (isPanelExpanded)
            {
                // Currently expanded, calculate base width before collapsing
                formBaseWidth = this.Width - panelWidthChange;
            }
            else
            {
                // Currently collapsed, current width is the base width before expanding
                formBaseWidth = this.Width;
            }

            Console.WriteLine($"Form base width for animation cycle: {formBaseWidth}");
            if (formBaseWidth <= 0 && this.WindowState == FormWindowState.Normal)
            {
                MessageBox.Show("Error: Cannot determine base width accurately.");
                return;
            }

            // Toggle the state for the *upcoming* animation
            isPanelExpanded = !isPanelExpanded;
            Console.WriteLine($"Animation triggered. Expanding: {isPanelExpanded}");
            panelAnimationStopwatch.Restart();
            panelAnimationTimer.Start();
            Console.WriteLine("Panel Animation Timer Started.");
        }
        private void PanelAnimationTimer_Tick(object sender, EventArgs e)
        {
            if (panelNav == null || pictureBox1 == null || pictureBoxStartX < 0) // Ensure pictureBox is ready
            {
                panelAnimationTimer.Stop(); return;
            }

            // Base width check (mostly relevant if state changes occur mid-animation, less critical now)
            if (formBaseWidth <= 0 && this.WindowState == FormWindowState.Normal)
            {
                Console.WriteLine("Warning: formBaseWidth invalid mid-animation.");
                // Might need recovery logic, but Form1_Resize should handle state changes better.
                // For now, we stop to prevent errors.
                panelAnimationTimer.Stop(); return;
            }

            TimeSpan elapsed = panelAnimationStopwatch.Elapsed;
            double progress = Math.Min(1.0, elapsed.TotalMilliseconds / panelAnimationDuration.TotalMilliseconds);
            double easedProgress = 1 - Math.Pow(1 - progress, 3); // Apply easing

            // --- Animate Panel Width ---
            int panelStartActualWidth = isPanelExpanded ? panelStartWidth : panelTargetWidth;
            int panelEndActualWidth = isPanelExpanded ? panelTargetWidth : panelStartWidth;
            int newPanelWidth = (int)(panelStartActualWidth + (panelEndActualWidth - panelStartActualWidth) * easedProgress);
            newPanelWidth = Math.Max(Math.Min(panelStartActualWidth, panelEndActualWidth), Math.Min(Math.Max(panelStartActualWidth, panelEndActualWidth), newPanelWidth));
            panelNav.Width = newPanelWidth;

            // --- Animate PictureBox Position ---
            int newPictureBoxLeft = newPanelWidth + pictureBoxStartX; // Position relative to panel's right edge
            pictureBox1.Left = newPictureBoxLeft;

            // --- Animate Form Width (Only When Expanding and if Normal State) ---
            if (isPanelExpanded && this.WindowState == FormWindowState.Normal && formBaseWidth > 0)
            {
                int panelWidthChange = panelTargetWidth - panelStartWidth;
                int formStartWidth = formBaseWidth; // Always starts from base when expanding
                int formEndWidth = formBaseWidth + panelWidthChange; // Ends at expanded width
                int newFormWidth = (int)(formStartWidth + (formEndWidth - formStartWidth) * easedProgress);
                // Clamp form width
                newFormWidth = Math.Max(formStartWidth, Math.Min(formEndWidth, newFormWidth)); // Clamp between base and expanded

                if (this.Width != newFormWidth)
                {
                    this.Width = newFormWidth;
                }
            }
            // --- NO Form Width animation when collapsing ---

            // --- Finish Animation ---
            if (progress >= 1.0)
            {
                panelAnimationTimer.Stop();
                panelAnimationStopwatch.Reset();

                // Set final states explicitly
                panelNav.Width = panelEndActualWidth;
                pictureBox1.Left = panelEndActualWidth + pictureBoxStartX; // Final PicBox position

                // Set final form width ONLY if expanding AND normal state
                if (isPanelExpanded && this.WindowState == FormWindowState.Normal && formBaseWidth > 0)
                {
                    int panelWidthChange = panelTargetWidth - panelStartWidth;
                    this.Width = formBaseWidth + panelWidthChange;
                }
                // --- DO NOT set form width when collapsing finishes ---

                // Update button image if applicable
                if (pictureBox1 != null) { try { pictureBox1.Image = isPanelExpanded ? Properties.Resources.Menu_Hovered : Properties.Resources.Menu; } catch { /* ignore */ } }

                panelNav.Invalidate();
                pictureBox1.Invalidate(); // Redraw picture box too
                Console.WriteLine("Panel Animation Timer Stopped (Finished).");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (panelNav != null && this.WindowState != FormWindowState.Minimized && pictureBox1 != null && pictureBoxStartX >= 0)
            {
                // Recalculate base width (width when panel is collapsed)
                int panelWidthChange = panelTargetWidth - panelStartWidth;
                formBaseWidth = this.Width - (isPanelExpanded ? panelWidthChange : 0);
                Console.WriteLine($"Form base width updated on resize ({this.WindowState}): {formBaseWidth}");

                // Reposition picturebox instantly on resize to match current panel state
                int currentPanelWidth = isPanelExpanded ? panelTargetWidth : panelStartWidth;
                // Handle edge case where animation might be running during resize
                if (panelAnimationTimer.Enabled)
                {
                    currentPanelWidth = panelNav.Width; // Use animating width
                }
                pictureBox1.Left = currentPanelWidth + pictureBoxStartX;
            }
        }
        // --- End Form Resize Event Handler ---

        // Remove Button1_Click if unused
        // private void Button1_Click(object sender, EventArgs e) { /* ... */ }

        #endregion


        #region Form_Load Event
        private async void Form1_Load(object sender, EventArgs e)
        {
            LoadFormIntoPanel(FormBlueprintDesigner);
            ScanForSensors();
            // --- Initialize Base Width for Animation ---
            // --- Initialize Base Width for Animation ---
            if (formBaseWidth <= 0)
            {
                formBaseWidth = this.Width; // Initial width IS the base width (panel starts collapsed)
                Console.WriteLine($"Form base width captured on load: {formBaseWidth}");
            }

            // --- Capture pictureBox1 Initial Position ---
            if (pictureBox1 != null)
            {
                // Assumes pictureBox1 starts positioned correctly relative to the *collapsed* panel edge (X=0)
                pictureBoxStartX = pictureBox1.Left;
                Console.WriteLine($"pictureBox1 initial Left position captured: {pictureBoxStartX}");
                if (pictureBoxStartX < 0) pictureBoxStartX = 5; // Fallback padding if calculation is odd
            }
            else
            {
                Console.WriteLine("Warning: pictureBox1 is null, cannot capture start position.");
            }

            // --- Initialize ESP32 Manager and Sensor Monitoring ---
            _espManager = new Esp32Manager(); // Instantiate the manager field
            try
            {
                _espManager.AddDevice(SensorDeviceId, "192.168.0.101");
                // Add more devices if needed...
                // _espManager.AddDevice("Sensor_Garage", "192.168.0.102");

                // Start the background sensor monitoring
                InitializeSensorMonitorTimer();

                // Optional: Get initial status immediately for faster UI update
                int initialStatus = await _espManager.GetSensorStatusAsync(SensorDeviceId);
                UpdateSensorStatusLabel(initialStatus); // Update UI right away
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing ESP32 manager or devices: {ex.Message}", "ESP32 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"ESP32 Initialization Error: {ex}");
            }
            try
            {
                _espManager.AddDevice(SensorDeviceId2, "192.168.0.102");
                // Add more devices if needed...
                // _espManager.AddDevice("Sensor_Garage", "192.168.0.102");

                // Start the background sensor monitoring
                InitializeSensorMonitorTimer();

                // Optional: Get initial status immediately for faster UI update
                int initialStatus = await _espManager.GetSensorStatusAsync(SensorDeviceId2);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing ESP32 manager or devices: {ex.Message}", "ESP32 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"ESP32 Initialization Error: {ex}");
            }


            // -------------------------------------------------------


            // --- Other Load Logic (SmartThings, etc.) ---
            // Example using placeholder method
            await LoadSmartThingsDataAsync(); // Encapsulate ST logic
            // ApplyRoundedCorners(); // Assuming this is UI related
            // NetworkScanner NS = new NetworkScanner(); // Your network scanner logic
            // await NS.ScanAndLogDeviceMacAsync(); // If needed


            Console.WriteLine("Form1_Load Complete.");
        }

        private async void ScanForSensors()
        {
            SensorScanner Scanner = new SensorScanner();

            Console.WriteLine("Starting network scan...");
            // Define the range: 192.168.0.200 to 192.168.0.250
            string baseIp = "192.168.0";
            int start = 100;
            int end = 110;
            int scanTimeout = 300; // milliseconds - adjust as needed

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
                }

                Console.WriteLine("\n---------------------");
                Console.WriteLine("Scan complete.");
                Console.WriteLine("---------------------");

                if (onlineDevices.Any())
                {
                    Console.WriteLine("Found the following reachable devices:");
                    foreach (var ip in onlineDevices.OrderBy(ip => ip.GetAddressBytes()[3])) // Order by last octet
                    {
                        Console.WriteLine($"- {ip}");
                    }
                }
                else
                {
                    Console.WriteLine("No devices found or responded in the specified range.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAn error occurred during the scan: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.ResetColor();
            }
        }

        // Example placeholder for SmartThings loading
        private async Task LoadSmartThingsDataAsync()
        {
            DeviceStatusResponse? fullStatus = null;
            string myPat = "9415ac5e-806f-4dd6-9c40-563e31f9e596"; // Load securely!
            string refrigeratorDeviceId = "9bda7531-b30c-f766-4846-0c27b89adc5b"; // Fridge ID

            // Use try-catch specifically around API calls
            try
            {
                using (var apiClient = new SmartThingsApiClient(myPat))
                {
                    Console.WriteLine($"Fetching status for device {refrigeratorDeviceId}...");
                    RefrigeratorStatus? status = await apiClient.GetRefrigeratorStatusAsync(refrigeratorDeviceId);

                    // Safely update UI or data models
                    if (FormBlueprintView is FormBlueprintView bpv) // Check type before casting
                    {
                        bpv.Fridge = status; // Assign status if view exists
                    }

                    if (status != null)
                    {
                        Console.WriteLine("Current Refrigerator Status:");
                        Console.WriteLine(status.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve or parse refrigerator status.");
                    }
                }
            }
            catch (ArgumentNullException argEx) // Catch specific error from missing token
            {
                MessageBox.Show($"SmartThings Configuration Error: {argEx.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine($"SmartThings setup error: {argEx}");
            }
            catch (Exception ex) // Catch other general errors during API calls
            {
                MessageBox.Show($"An error occurred loading SmartThings data: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine($"SmartThings load error: {ex}");
            }
            // Simulate async work if needed
            await Task.Delay(10); // Placeholder
        }

        #endregion


        #region Sensor Monitoring Timer (Correct Implementation)

        private void InitializeSensorMonitorTimer()
        {
            if (_espManager == null || _espManager.GetDeviceController(SensorDeviceId) == null)
            {
                Console.WriteLine($"Cannot start monitoring: ESP Manager not ready or device '{SensorDeviceId}' not added.");
                return; // Don't start timer if manager or device isn't set up
            }

            // Dispose previous timer if re-initializing (defensive coding)
            _sensorMonitorTimer?.Dispose();

            _sensorMonitorTimer = new System.Timers.Timer(SensorMonitorIntervalMs);
            _sensorMonitorTimer.Elapsed += SensorMonitorTimer_Elapsed; // Wire up the event handler
            _sensorMonitorTimer.AutoReset = true; // Keep firing the event
            _sensorMonitorTimer.Enabled = true;  // Start the timer
            Console.WriteLine($"Started monitoring '{SensorDeviceId}' every {SensorMonitorIntervalMs / 1000} seconds.");
        }

        // Runs on a background thread pool thread
        private async void SensorMonitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Optional: Prevent re-entrancy if the check takes longer than the interval
            // Can use Monitor.TryEnter/Exit or a simple boolean flag
            // For simplicity, omitted here, but consider if GetSensorStatusAsync is slow.

            try
            {
                if (_espManager != null)
                {
                    // Fetch the status asynchronously - doesn't block UI thread
                    int currentStatus = await _espManager.GetSensorStatusAsync(SensorDeviceId);

                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Monitored '{SensorDeviceId}' Status: {currentStatus} {(currentStatus == -1 ? "(Error/Timeout)" : "")}");

                    // --- Update UI Safely ---
                    // Marshal the call back to the UI thread.
                    UpdateSensorStatusLabel(currentStatus);
                }
            }
            catch (ObjectDisposedException)
            {
                // Manager or timer might have been disposed during shutdown, ignore.
                Debug.WriteLine("Sensor monitor timer elapsed after relevant object disposal.");
            }
            catch (Exception ex)
            {
                // Log any errors during the sensor check
                Console.WriteLine($"Error during sensor monitoring for '{SensorDeviceId}': {ex.Message}");
                // Optionally update UI to show a general monitoring error state
                UpdateSensorStatusLabel(-2); // Use a specific code for monitoring errors
            }
        }

        // Helper Method to Marshal UI Update
        private void UpdateSensorStatusLabel(int status)
        {
            // Check if the control exists and if invoke is required (i.e., called from background thread)
            // Also check if the form handle is created (form is actually visible/loaded)
            if (lblSensorStatus != null && this.IsHandleCreated)
            {
                if (this.InvokeRequired)
                {
                    try
                    {
                        // Use BeginInvoke for potentially faster return to the background thread,
                        // or Invoke if you need to wait for the UI update to complete.
                        this.BeginInvoke((MethodInvoker)delegate { UpdateLabelText(status); });
                    }
                    catch (ObjectDisposedException) { /* Control or form disposed, ignore */ }
                    catch (InvalidOperationException) { /* Handle might have been destroyed, ignore */ }
                }
                else // Already on UI thread or safe to call directly
                {
                    UpdateLabelText(status);
                }
            }
            else if (!this.IsHandleCreated)
            {
                Debug.WriteLine($"Sensor update skipped: Handle not created. Status={status}");
            }
            else if (lblSensorStatus == null)
            {
                Debug.WriteLine($"Sensor update skipped: lblSensorStatus is null. Status={status}");
            }
        }

        // Actual UI update logic - MUST run on the UI thread
        private async void UpdateLabelText(int status)
        {
            // Double-check control existence within the UI thread context
            if (lblSensorStatus == null) return;

            string statusText;
            Color statusColor;

            switch (status)
            {
                case 0:
                    statusText = "Idle/Closed"; // Customize text
                    await _espManager.SetBuzzerStateAsync(SensorDeviceId, false);
                    statusColor = Color.Lime;
                    break;
                case 1:
                    statusText = "ACTIVE/Open"; // Customize text
                    await _espManager.SetBuzzerStateAsync(SensorDeviceId, true);
                    statusColor = Color.Red;
                    break;
                case -1:
                    statusText = "Error/Timeout";
                    statusColor = Color.OrangeRed;
                    break;
                case -2:
                    statusText = "Monitor Error";
                    statusColor = Color.DarkGray;
                    break;
                default:
                    statusText = $"Unknown ({status})";
                    statusColor = Color.Gray;
                    break;
            }
            lblSensorStatus.Text = $"Sensor: {statusText}"; // Simplified text example
            lblSensorStatus.ForeColor = statusColor;
        }

        #endregion


        #region Form Loading / UI Interaction (LoadFormIntoPanel, Drag/Drop, etc.)
        // (Keep your existing methods: LoadFormIntoPanel, Form1_MouseDown, Form1_MouseMove,
        //  Form1_MouseUp, Form1_MouseDoubleClick, pictureBox1_MouseDown, pictureBox1_MouseUp,
        //  buttonBlueprint_Click, buttonSettings_Click here)

        private void LoadFormIntoPanel(Form childForm)
        {
            if (panelMain == null) { MessageBox.Show("Error: panelMain is null."); return; }
            if (childForm == null) { MessageBox.Show("Error: childForm is null."); return; }
            if (activeChildForm == childForm) { childForm.BringToFront(); return; }
            activeChildForm?.Close();
            panelMain.Controls.Clear();
            activeChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { isDragging = true; dragStartPoint = new Point(e.X, e.Y); }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging) { Point p = PointToScreen(e.Location); Location = new Point(p.X - dragStartPoint.X, p.Y - dragStartPoint.Y); }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { isDragging = false; }
        }
        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = (this.WindowState == FormWindowState.Normal) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1 != null && Properties.Resources.Menu_Hovered != null) pictureBox1.Image = Properties.Resources.Menu_Hovered;
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1 != null && Properties.Resources.Menu != null) pictureBox1.Image = Properties.Resources.Menu;
        }
        private void buttonBlueprint_Click(object sender, EventArgs e)
        {
            if (FormBlueprintView != null && activeChildForm?.GetType() != typeof(FormBlueprintView)) { LoadFormIntoPanel(FormBlueprintView); }
        }
        private void buttonSettings_Click(object sender, EventArgs e)
        {
            if (FormBlueprintDesigner != null && activeChildForm?.GetType() != typeof(FormBlueprintDesigner)) { LoadFormIntoPanel(FormBlueprintDesigner); }
        }
        #endregion


        // This handler should be automatically called if hooked up in Constructor or Designer
        #region Form Closing / Cleanup
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Form1_FormClosing executing...");

            // Unsubscribe from events to prevent issues after disposal
            this.Resize -= Form1_Resize; // <<< ADD THIS LINE
            if (pictureBox1 != null) pictureBox1.Click -= pictureBox1_Click; // Good practice
                                                                             // Add any other event unsubscriptions here

            // ... rest of your existing cleanup code for timers, _espManager etc. ...

            Console.WriteLine("Form1_FormClosing complete.");
        }
        #endregion

        // KEEP this method and ensure it's linked via the Designer Properties -> Events -> DoubleClick
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else // Covers Maximized (restores to Normal)
            {
                this.WindowState = FormWindowState.Normal;
            }
            // Note: The Form1_Resize handler (added below) will ensure panelNav adjusts height.
        }

        // DELETE this method entirely if it still exists in your code:
        // private void Form1_MouseDoubleClick(object sender, MouseEventArgs e) { ... }

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            LoadFormIntoPanel(FormSettings);
        }
    } // End Form1 Class
} // End Namespace