using Security.Blueprints;
using Security.SmartThings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Security.SmartThings.SmartThingsApiClient;

namespace Security
{
    public partial class FormBlueprintView : Form
    {
        public Storage Storage { get; set; }
        public BlueprintDesign Design { get; set; }
        public RefrigeratorStatus Fridge { get; set; }
        public FormBlueprintView(Storage storage, BlueprintDesign design)
        {
            Storage = storage;
            Design = design;
            InitializeComponent();
            if (!string.IsNullOrEmpty(Design.BackgroundImage) && System.IO.File.Exists(Design.BackgroundImage))
            {
                this.BackgroundImageLayout = ImageLayout.Stretch;
                this.BackgroundImage = Image.FromFile(Design.BackgroundImage);
            }
        }

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
                pbNewCam.Click += PbNewCam_Click;
                pbNewCam.Cursor = Cursors.Hand;

                // Add the Control to the Form's controls collection
                this.Controls.Add(pbNewCam);
                pbNewCam.BringToFront();
                Console.WriteLine($"Loaded control '{pbNewCam.Name}' at {pbNewCam.Location}");
            }

            //labelFreezerTemp.Text = Fridge.FreezerTemperatureF.ToString() + " F";
            //labelFridgeTemp.Text = Fridge.FridgeTemperatureF.ToString() + " F";
        }

        string myPat = "9415ac5e-806f-4dd6-9c40-563e31f9e596"; // Load this securely!
        string refrigeratorDeviceId = "9bda7531-b30c-f766-4846-0c27b89adc5b"; // Fridge ID
        private void PbNewCam_Click(object? sender, EventArgs e)
        {
            Camera cam = (Camera)(((PictureBox)sender).Tag);

            if (vlcControl1 != null && !vlcControl1.IsPlaying)
            {
                string rtspUrl = cam.RTSPAddress;
                vlcControl1.Play(new Uri(rtspUrl));
                vlcControl1.VlcMediaPlayer.Audio.IsMute = !cam.PlaySound;
                panelCam.Visible = true;
                panelCam.BringToFront();
            }
            else if (vlcControl1 != null)
            {
                // Handle cases where control exists but isn't initialized (rare if setup is correct)
                MessageBox.Show("VLC Control is not initialized. Cannot play stream.", "VLC Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Handle case where vlcControl1 is unexpectedly null
                MessageBox.Show("VLC Control reference is null. Check designer code.", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        private void FormBlueprintView_Load(object sender, EventArgs e)
        {
            smart = new SmartThingsApiClient(myPat);
            //if (vlcControl1 != null && !vlcControl1.IsPlaying)
            //{
            //    string rtspUrl = Storage.Cameras[0].RTSPAddress;
            //    vlcControl1.Play(new Uri(rtspUrl));
            //    vlcControl1.VlcMediaPlayer.Audio.IsMute = !Storage.Cameras[0].PlaySound;
            //    vlcControl3.Play(new Uri(rtspUrl));
            //    vlcControl3.VlcMediaPlayer.Audio.IsMute = !Storage.Cameras[0].PlaySound;
            //}
            //else if (vlcControl1 != null)
            //{
            //    // Handle cases where control exists but isn't initialized (rare if setup is correct)
            //    MessageBox.Show("VLC Control is not initialized. Cannot play stream.", "VLC Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            //{
            //    // Handle case where vlcControl1 is unexpectedly null
            //    MessageBox.Show("VLC Control reference is null. Check designer code.", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            LoadCurrentDesignControls();
        }

        private void pbExit_Click(object sender, EventArgs e)
        {
            panelCam.Visible = false;
        }

        private void pbSmartThings_Click(object sender, EventArgs e)
        {
            if (panelSmartThings.Visible)
            {
                panelSmartThings.Visible = false;
            }
            else
            {
                panelSmartThings.Visible = true;
            }
        }

        SmartThingsApiClient smart = null; 
        
        private void UpdateUILabels()
        {
            if (Fridge == null)
            {
                labelFreezerTemp.Text = "N/A";
                labelFridgeTemp.Text = "N/A";
                // Maybe disable buttons if status is unknown?
                return;
            }

            // Use null-conditional operator ?. and null-coalescing operator ??
            // Format the temperature string
            labelFreezerTemp.Text = $"{Fridge.FreezerTemperatureF.ToString("F0") ?? "N/A"} °F";
            labelFridgeTemp.Text = $"{Fridge.FridgeTemperatureF.ToString("F0") ?? "N/A"} °F";
        }

        // --- Helper to Refresh Status and Update UI ---
        private async Task RefreshFridgeStatusAsync()
        {
            if (smart == null || string.IsNullOrEmpty(refrigeratorDeviceId))
            {
                MessageBox.Show("API Client or Device ID not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Consider adding UI feedback (e.g., "Loading...", disable buttons)
            try
            {
                Fridge = await smart.GetRefrigeratorStatusAsync(refrigeratorDeviceId);
                if (Fridge == null)
                {
                    MessageBox.Show("Failed to retrieve refrigerator status from API.", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Fridge = null; // Clear status on error
                MessageBox.Show($"Error fetching status: {ex.Message}", "API Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Log the full exception: Log.Error(ex, "Failed to get fridge status");
            }
            finally
            {
                UpdateUILabels();
                // Re-enable buttons if they were disabled
            }
        }


        // --- Refactored Temperature Change Logic ---
        private async Task ChangeTemperatureAndRefresh(Func<double?, Task<bool>> setTemperatureAction, double? currentTemperature, int delta)
        {
            if (smart == null || string.IsNullOrEmpty(refrigeratorDeviceId))
            {
                MessageBox.Show("API Client or Device ID not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!currentTemperature.HasValue)
            {
                MessageBox.Show("Current temperature is unknown. Cannot change.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Or try refreshing status first?
            }

            double newTemperature = currentTemperature.Value + delta;
            bool success = false;

            // Disable UI temporarily
            ToggleTemperatureButtons(false);

            try
            {
                success = await setTemperatureAction(newTemperature); // Call the specific Set... method

                if (success)
                {
                    // Optimistic UI Update (Optional but good for responsiveness)
                    // UpdateUILabelsWithOptimisticValue(..., newTemperature);

                    await Task.Delay(5000); // Wait longer (5 seconds) for state to hopefully update in cloud
                    await RefreshFridgeStatusAsync(); // Fetch the actual status
                }
                else
                {
                    MessageBox.Show("Failed to send temperature change command to API.", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await RefreshFridgeStatusAsync(); // Refresh to show the actual current state
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing temperature: {ex.Message}", "API Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await RefreshFridgeStatusAsync(); // Refresh to show the actual current state
                                                  // Log the full exception
            }
            finally
            {
                // Re-enable UI
                ToggleTemperatureButtons(true);
            }
        }

        // Helper to enable/disable all temp buttons
        private void ToggleTemperatureButtons(bool enabled)
        {
            buttonFridgeTempDown.Enabled = enabled;
            buttonFridgeTempUp.Enabled = enabled;
            buttonFreezerTempDown.Enabled = enabled;
            buttonFreezerTempUp.Enabled = enabled;
            // Potentially add a visual cue like a spinner/loading indicator
        }


        // --- Event Handlers (Now much simpler) ---

        private async void buttonFridgeTempDown_Click(object sender, EventArgs e)
        {
            // Pass the correct API method using a lambda
            await ChangeTemperatureAndRefresh(
                (newTemp) => smart!.SetFridgeTemperatureAsync(refrigeratorDeviceId!, newTemp!.Value), // Non-null asserted as checks are done in ChangeTemperatureAndRefresh
                Fridge?.FridgeTemperatureF,
                -1 // Delta
            );
        }

        private async void buttonFreezerTempDown_Click(object sender, EventArgs e)
        {
            await ChangeTemperatureAndRefresh(
                (newTemp) => smart!.SetFreezerTemperatureAsync(refrigeratorDeviceId!, newTemp!.Value),
                Fridge?.FreezerTemperatureF,
                -1 // Delta
            );
        }

        private async void buttonFridgeTempUp_Click(object sender, EventArgs e)
        {
            await ChangeTemperatureAndRefresh(
                (newTemp) => smart!.SetFridgeTemperatureAsync(refrigeratorDeviceId!, newTemp!.Value),
                Fridge?.FridgeTemperatureF,
                +1 // Delta
            );
        }

        private async void buttonFreezerTempUp_Click(object sender, EventArgs e)
        {
            await ChangeTemperatureAndRefresh(
                (newTemp) => smart!.SetFreezerTemperatureAsync(refrigeratorDeviceId!, newTemp!.Value),
                Fridge?.FreezerTemperatureF,
                +1 // Delta
            );
        }
    }
}
