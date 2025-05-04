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
using Security.Settings;

namespace Security
{
    // new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "libvlc", (IntPtr.Size == 4 ? "win-x86" : "win-x64")));
    public partial class FormMain : Form
    {
        #region Variables

        // --- UI State ---
        private bool isDragging = false;
        private Point dragOffset;     
        private Form activeChildForm = null;

        // --- Sub Forms ---
        private Form FormBlueprintView = null;
        private Form FormBlueprintDesigner = null;
        private FormSettings FormSettings = null;

        // --- Sensor Monitoring ---
        private Esp32Manager Esp32Manager;                    
        private System.Timers.Timer _sensorMonitorTimer;      
        private const string SensorDeviceId = "Sensor_LivingRoom"; 
        private const string SensorDeviceId2 = "Sensor_FrontDoor"; 
        private const double SensorMonitorIntervalMs = 5000;  

        // --- PanelNav Size Animation ---
        private System.Windows.Forms.Timer panelAnimationTimer;
        private Stopwatch panelAnimationStopwatch = new Stopwatch();
        private const int panelStartWidth = 0;          
        private int panelTargetWidth = 250;             
        private TimeSpan panelAnimationDuration = TimeSpan.FromMilliseconds(350);
        private bool isPanelExpanded = false;           
        private int formBaseWidth = -1;            
        private int formTargetExpandedWidth = -1; 
        private int pictureBoxStartX = -1;

        #endregion

        #region Public Properties

        public Storage Storage { get; set; } = null;
        public BlueprintDesign BlueprintDesign { get; set; } = null;

        #endregion

        #region Constructor
        public FormMain()
        {
            Storage = Storage.Load();
            BlueprintDesign = BlueprintDesign.Load();
            InitializeComponent(); 
            InitializeSubForms();
            InitializePanelAnimationTimer();

            if (panelNav != null) panelNav.Width = panelStartWidth;

            this.FormClosing += Form1_FormClosing;
            this.Resize += Form1_Resize; 

            if (pictureBox1 != null) pictureBox1.Click += pictureBox1_Click;
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
            if (panelNav == null || panelAnimationTimer.Enabled || formBaseWidth <= 0) // Removed formTargetExpandedWidth check
            {
                Console.WriteLine($"Animation trigger ignored: Panel({panelNav == null}), Timer({panelAnimationTimer.Enabled}), PicBox(), BaseWidth({formBaseWidth <= 0})");
                return;
            }

            int panelWidthChange = panelTargetWidth - panelStartWidth;

            // Update formBaseWidth based on the state *before* toggling
            if (isPanelExpanded)
            {
                // We are currently expanded, about to collapse. Update base width based on current size.
                formBaseWidth = this.Width - panelWidthChange;
                Console.WriteLine($"Recalculated form base width before collapse: {formBaseWidth}");
            }
            else
            {
                // We are currently collapsed, about to expand. Update base width if needed (e.g., user resized while collapsed).
                // Check Form1_Resize logic for this update. If Resize is correct, this might not be needed here.
                // For safety, let's ensure it reflects current width if collapsed:
                formBaseWidth = this.Width;
                Console.WriteLine($"Using current width as form base width before expansion: {formBaseWidth}");
            }

            // Safety check after potential recalculation
            if (formBaseWidth <= 0 && this.WindowState == FormWindowState.Normal)
            {
                MessageBox.Show("Error: Base width became invalid before animation.");
                return;
            }

            // Toggle the state for the *upcoming* animation
            isPanelExpanded = !isPanelExpanded;
            Console.WriteLine($"Animation triggered. Expanding/Collapsing: {isPanelExpanded}"); // Updated log text
            panelAnimationStopwatch.Restart();
            panelAnimationTimer.Start();
            Console.WriteLine("Panel Animation Timer Started.");
        }

        private void PanelAnimationTimer_Tick(object sender, EventArgs e)
        {
            if (panelNav == null) { panelAnimationTimer.Stop(); return; }
            if (formBaseWidth <= 0)
            {
                Console.WriteLine("Error: Invalid formBaseWidth during animation tick.");
                panelAnimationTimer.Stop();
                return;
            }

            TimeSpan elapsed = panelAnimationStopwatch.Elapsed;
            double progress = Math.Min(1.0, elapsed.TotalMilliseconds / panelAnimationDuration.TotalMilliseconds);
            double easedProgress = 1 - Math.Pow(1 - progress, 3);

            // --- Animate Panel Width (Same as before) ---
            int panelStartActualWidth = isPanelExpanded ? panelStartWidth : panelTargetWidth;
            int panelEndActualWidth = isPanelExpanded ? panelTargetWidth : panelStartWidth;
            int newPanelWidth = (int)(panelStartActualWidth + (panelEndActualWidth - panelStartActualWidth) * easedProgress);
            newPanelWidth = Math.Max(Math.Min(panelStartActualWidth, panelEndActualWidth), Math.Min(Math.Max(panelStartActualWidth, panelEndActualWidth), newPanelWidth));
            panelNav.Width = newPanelWidth;

            // --- Animate PictureBox Position (Same as before) ---
            int newPictureBoxLeft = newPanelWidth + pictureBoxStartX;

            // --- Animate Form Width (NOW FOR BOTH EXPAND AND COLLAPSE if Normal State) ---
            if (this.WindowState == FormWindowState.Normal)
            {
                int panelWidthChange = panelTargetWidth - panelStartWidth;
                int formStartWidth, formEndWidth;

                if (isPanelExpanded) // Expanding
                {
                    formStartWidth = formBaseWidth;
                    formEndWidth = formBaseWidth + panelWidthChange;
                }
                else // Collapsing
                {
                    // Start from the current expanded width (base + change)
                    formStartWidth = formBaseWidth + panelWidthChange;
                    // End at the base width
                    formEndWidth = formBaseWidth;
                }

                int newFormWidth = (int)(formStartWidth + (formEndWidth - formStartWidth) * easedProgress);
                // Clamp form width between the start and end points for this animation direction
                newFormWidth = Math.Max(Math.Min(formStartWidth, formEndWidth), Math.Min(Math.Max(formStartWidth, formEndWidth), newFormWidth));

                if (this.Width != newFormWidth)
                {
                    this.Width = newFormWidth;
                }
            }

            // --- Finish Animation ---
            if (progress >= 1.0)
            {
                panelAnimationTimer.Stop();
                panelAnimationStopwatch.Reset();

                // Set final panel and picturebox states
                panelNav.Width = panelEndActualWidth;

                // Set final form width based on the new state (if Normal)
                if (this.WindowState == FormWindowState.Normal)
                {
                    int panelWidthChange = panelTargetWidth - panelStartWidth;
                    int finalFormWidth = isPanelExpanded ? formBaseWidth + panelWidthChange : formBaseWidth;
                    this.Width = finalFormWidth;
                    Console.WriteLine($"Animation finished. isPanelExpanded={isPanelExpanded}. Form Width set to: {finalFormWidth}");
                }

                // Update button image
                if (pictureBox1 != null) { try { pictureBox1.Image = isPanelExpanded ? Properties.Resources.Menu_Hovered1 : Properties.Resources.Menu1; } catch { /* ignore */ } }

                panelNav.Invalidate();
                pictureBox1.Invalidate();
                Console.WriteLine("Panel Animation Timer Stopped (Finished).");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || panelNav == null)
            {
                return; // Ignore on minimize or if controls not ready
            }

            int panelWidthChange = panelTargetWidth - panelStartWidth;

            // Update base width based on the NEW form width and CURRENT panel state
            if (isPanelExpanded && !panelAnimationTimer.Enabled) // Panel is fully expanded
            {
                // The current width is the expanded width, calculate the base width from it
                formBaseWidth = this.Width - panelWidthChange;
                Console.WriteLine($"Form base width updated on resize (Panel Expanded): {formBaseWidth}");
            }
            else if (!isPanelExpanded && !panelAnimationTimer.Enabled) // Panel is fully collapsed
            {
                // The current width IS the new base width
                formBaseWidth = this.Width;
                Console.WriteLine($"Form base width updated on resize (Panel Collapsed): {formBaseWidth}");
            }
            // Else: If animating, don't update base width here, let the click handler manage it

            // Always reposition picturebox instantly based on current panel width
            int currentPanelWidth = panelNav.Width;
            if (!panelAnimationTimer.Enabled) // If not animating, use the definite state width
            {
                currentPanelWidth = isPanelExpanded ? panelTargetWidth : panelStartWidth;
            }
            Console.WriteLine($"Form resized ({this.WindowState}). PictureBox Left: {pictureBox1.Left}");
        }
        // --- End Form Resize Event Handler ---

        // Remove Button1_Click if unused
        // private void Button1_Click(object sender, EventArgs e) { /* ... */ }

        #endregion

        #region Sensor Monitoring Timer (Correct Implementation)

        private void InitializeSensorMonitorTimer()
        {
            if (Esp32Manager == null || Esp32Manager.GetDeviceController(SensorDeviceId) == null)
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
        private async void SensorMonitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Optional: Prevent re-entrancy if the check takes longer than the interval
            // Can use Monitor.TryEnter/Exit or a simple boolean flag
            // For simplicity, omitted here, but consider if GetSensorStatusAsync is slow.

            try
            {
                if (Esp32Manager != null)
                {
                    // Fetch the status asynchronously - doesn't block UI thread
                    int currentStatus = await Esp32Manager.GetSensorStatusAsync(SensorDeviceId);

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
                    await Esp32Manager.SetBuzzerStateAsync(SensorDeviceId, false);
                    statusColor = Color.Lime;
                    break;
                case 1:
                    statusText = "ACTIVE/Open"; // Customize text
                    await Esp32Manager.SetBuzzerStateAsync(SensorDeviceId, true);
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

        #region Helper Functions

        private void InitializeSubForms()
        {
            FormBlueprintView = new FormBlueprintView(Storage, BlueprintDesign);
            FormBlueprintDesigner = new FormBlueprintDesigner(BlueprintDesign, Storage);
            FormSettings = new FormSettings(Storage);
        }
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

        #endregion

        #region Form Events

        #region Form_Load Event

        private async void Form1_Load(object sender, EventArgs e)
        {
            LoadFormIntoPanel(FormBlueprintView);

            if (formBaseWidth <= 0) // Only on first load
            {
                formBaseWidth = this.Width; // Initial width IS the base width (panel starts collapsed)
                Console.WriteLine($"Form base width captured on load: {formBaseWidth}");
            }

            if (pictureBox1 != null)
            {
                pictureBoxStartX = pictureBox1.Left;
                Console.WriteLine($"pictureBox1 initial Left position captured: {pictureBoxStartX}");
                if (pictureBoxStartX < 0) pictureBoxStartX = 5;
            }
            else
            {
                Console.WriteLine("Warning: pictureBox1 is null.");
            }

            // --- Initialize ESP32 Manager and Sensor Monitoring ---
            Esp32Manager = new Esp32Manager(); // Instantiate the manager field
            try
            {
                Esp32Manager.AddDevice(SensorDeviceId, "192.168.0.101");
                // Add more devices if needed...
                // Esp32Manager.AddDevice("Sensor_Garage", "192.168.0.102");

                // Start the background sensor monitoring
                InitializeSensorMonitorTimer();

                // Optional: Get initial status immediately for faster UI update
                int initialStatus = await Esp32Manager.GetSensorStatusAsync(SensorDeviceId);
                UpdateSensorStatusLabel(initialStatus); // Update UI right away
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing ESP32 manager or devices: {ex.Message}", "ESP32 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"ESP32 Initialization Error: {ex}");
            }
            try
            {
                Esp32Manager.AddDevice(SensorDeviceId2, "192.168.0.102");
                // Add more devices if needed...
                // Esp32Manager.AddDevice("Sensor_Garage", "192.168.0.102");

                // Start the background sensor monitoring
                InitializeSensorMonitorTimer();

                // Optional: Get initial status immediately for faster UI update
                int initialStatus = await Esp32Manager.GetSensorStatusAsync(SensorDeviceId2);
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

        #region Form Closing / Cleanup
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Form1_FormClosing executing...");

            // Unsubscribe from events to prevent issues after disposal
            this.Resize -= Form1_Resize; // <<< ADD THIS LINE
            if (pictureBox1 != null) pictureBox1.Click -= pictureBox1_Click; // Good practice
                                                                             // Add any other event unsubscriptions here

            // ... rest of your existing cleanup code for timers, Esp32Manager etc. ...

            Console.WriteLine("Form1_FormClosing complete.");
        }
        #endregion

        #region Form Maximize
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
        }
        #endregion

        #region Form Drag

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

        #endregion

        #endregion

        #region Navigation Event Handlers

        private void buttonBlueprint_Click(object sender, EventArgs e)
        {
            FormBlueprintView = new FormBlueprintView(Storage, BlueprintDesign);
            if (FormBlueprintView != null && activeChildForm?.GetType() != typeof(FormBlueprintView)) { LoadFormIntoPanel(FormBlueprintView); }
            pictureBox1_Click(pictureBox1, e);
        }
        private void buttonSettings_Click(object sender, EventArgs e)
        {
            FormBlueprintDesigner = new FormBlueprintDesigner(BlueprintDesign, Storage);
            if (FormBlueprintDesigner != null && activeChildForm?.GetType() != typeof(FormBlueprintDesigner)) { LoadFormIntoPanel(FormBlueprintDesigner); }
            pictureBox1_Click(pictureBox1, e);
        }
        private void buttonSetting_Click(object sender, EventArgs e)
        {
            FormSettings = new FormSettings(Storage);
            LoadFormIntoPanel(FormSettings);
            pictureBox1_Click(pictureBox1, e);
        }
        private void pbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Navigation Buton
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1 != null && Properties.Resources.Menu_Hovered1 != null) pictureBox1.Image = Properties.Resources.Menu_Hovered1;
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1 != null && Properties.Resources.Menu1 != null) pictureBox1.Image = Properties.Resources.Menu1;
        }

        #endregion

    } 
} 