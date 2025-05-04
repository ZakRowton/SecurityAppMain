using System; // Required for EventArgs
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Security
{
    partial class Form1
    {
        private int borderRadius = 25; // Adjust the radius for rounded corners
        private Color borderColor = Color.Purple; // Start color for gradient border
        private Color borderEndColor = Color.Gold; // End color for gradient border
        private int borderWidth = 3; // Width for the border
        private float opacity = 0.85f; // Opacity of the form

        // --- Animation Fields ---
        private System.Windows.Forms.Timer animationTimer;
        private float gradientOffset = 0.0f;
        private float animationSpeed = 0.015f; // Controls how fast the gradient moves (adjust as needed)
        // -------------------------

        // --- P/Invoke for Drop Shadow ---
        private const int CS_DROPSHADOW = 0x00020000;

        // Override CreateParams to add the drop shadow class style
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

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
            if (disposing)
            {
                // --- Dispose the timer ---
                if (animationTimer != null)
                {
                    animationTimer.Stop();
                    animationTimer.Dispose();
                    animationTimer = null;
                }

                // Dispose timers
                if (panelAnimationTimer != null)
                {
                    panelAnimationTimer.Stop();
                    // No explicit Dispose needed if in components container
                    panelAnimationTimer = null;
                }

                // Dispose components (includes timers if added via designer)
                components?.Dispose();

                // Unhook event handlers manually
                if (this.panelNav != null)
                {
                    this.panelNav.Paint -= PanelNav_Paint;
                    this.panelNav.Resize -= (sender, ev) => panelNav.Invalidate(); // Unhook resize lambda
                }
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
            animationTimer = new System.Windows.Forms.Timer(components);
            panelNav = new Panel();
            buttonSetting = new Button();
            lblSensorStatus = new Label();
            buttonSettings = new Button();
            buttonBlueprint = new Button();
            pictureBox1 = new PictureBox();
            panelMain = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            panelNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // animationTimer
            // 
            animationTimer.Enabled = true;
            animationTimer.Interval = 16;
            animationTimer.Tick += AnimationTimer_Tick;
            // 
            // panelNav
            // 
            panelNav.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panelNav.BackColor = Color.DimGray;
            panelNav.BorderStyle = BorderStyle.FixedSingle;
            panelNav.Controls.Add(buttonSetting);
            panelNav.Controls.Add(lblSensorStatus);
            panelNav.Controls.Add(buttonSettings);
            panelNav.Controls.Add(buttonBlueprint);
            panelNav.Location = new Point(0, 0);
            panelNav.Margin = new Padding(3, 4, 3, 4);
            panelNav.Name = "panelNav";
            panelNav.Size = new Size(10, 1077);
            panelNav.TabIndex = 2;
            // 
            // buttonSetting
            // 
            buttonSetting.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            buttonSetting.Location = new Point(32, 389);
            buttonSetting.Margin = new Padding(3, 4, 3, 4);
            buttonSetting.Name = "buttonSetting";
            buttonSetting.Size = new Size(167, 45);
            buttonSetting.TabIndex = 3;
            buttonSetting.Text = "Settings";
            buttonSetting.UseVisualStyleBackColor = true;
            buttonSetting.Click += buttonSetting_Click;
            // 
            // lblSensorStatus
            // 
            lblSensorStatus.AutoSize = true;
            lblSensorStatus.Location = new Point(40, 704);
            lblSensorStatus.Name = "lblSensorStatus";
            lblSensorStatus.Size = new Size(50, 20);
            lblSensorStatus.TabIndex = 2;
            lblSensorStatus.Text = "label1";
            // 
            // buttonSettings
            // 
            buttonSettings.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            buttonSettings.Location = new Point(32, 229);
            buttonSettings.Margin = new Padding(3, 4, 3, 4);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(167, 45);
            buttonSettings.TabIndex = 1;
            buttonSettings.Text = "Designer";
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += buttonSettings_Click;
            // 
            // buttonBlueprint
            // 
            buttonBlueprint.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            buttonBlueprint.Location = new Point(32, 92);
            buttonBlueprint.Margin = new Padding(3, 4, 3, 4);
            buttonBlueprint.Name = "buttonBlueprint";
            buttonBlueprint.Size = new Size(167, 45);
            buttonBlueprint.TabIndex = 0;
            buttonBlueprint.Text = "Blueprint";
            buttonBlueprint.UseVisualStyleBackColor = true;
            buttonBlueprint.Click += buttonBlueprint_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(64, 64, 64);
            pictureBox1.Image = Properties.Resources.Menu;
            pictureBox1.Location = new Point(16, 7);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(42, 47);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // panelMain
            // 
            panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panelMain.Location = new Point(368, 7);
            panelMain.Margin = new Padding(3, 4, 3, 4);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1432, 1063);
            panelMain.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1803, 1077);
            Controls.Add(pictureBox1);
            Controls.Add(panelNav);
            Controls.Add(panelMain);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Opacity = 0.9D;
            Text = "Form1";
            Load += Form1_Load;
            DoubleClick += Form1_DoubleClick;
            MouseDoubleClick += Form1_MouseDoubleClick;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;
            panelNav.ResumeLayout(false);
            panelNav.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            // -----------------------------------------
        }

        #endregion

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            gradientOffset += animationSpeed;
            if (gradientOffset > 4.0f)
            {
                gradientOffset -= 4.0f;
            }
            // Invalidate the whole control - DoubleBuffered handles flicker reduction
            this.Invalidate();
        }

        private GraphicsPath CreateRoundedRectPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (diameter > bounds.Width) diameter = bounds.Width;
            if (diameter > bounds.Height) diameter = bounds.Height;

            if (diameter <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ApplyRoundedCorners()
        {
            // Added check for Zero width/height before creating region
            if (this.IsHandleCreated && this.Width > 0 && this.Height > 0)
            {
                try
                {
                    using (GraphicsPath path = CreateRoundedRectPath(this.ClientRectangle, borderRadius))
                    {
                        // Check if Region needs replacing or setting initially
                        Region currentRegion = this.Region;
                        this.Region = new Region(path); // Create and assign the new region
                        currentRegion?.Dispose(); // Dispose the old region *after* assigning the new one
                    }
                }
                catch (Exception ex) { /* Handle or log potential GDI exceptions if necessary */ }
            }
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Invalidate before applying corners during resize might help reduce artifacts
            // this.Invalidate();
            ApplyRoundedCorners();
            // Ensure redraw after resize and region change
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Use high quality settings for graphics
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // Already set, but good practice

            if (borderWidth > 0 && borderRadius > 0 && this.Width > borderWidth && this.Height > borderWidth) // Added check width/height > borderwidth
            {
                Rectangle borderRect = new Rectangle(
                    borderWidth / 2,
                    borderWidth / 2,
                    this.Width - borderWidth,
                    this.Height - borderWidth);

                // Check for zero or negative dimensions which can cause GDI+ errors
                if (borderRect.Width <= 0 || borderRect.Height <= 0) return;

                int adjustedRadius = borderRadius - (borderWidth / 2);
                // Ensure radius isn't negative after adjustment
                if (adjustedRadius < 0) adjustedRadius = 0;

                using (GraphicsPath borderPath = CreateRoundedRectPath(borderRect, adjustedRadius))
                {
                    Rectangle gradientRect = new Rectangle(
                        borderRect.X,
                        borderRect.Y,
                        borderRect.Width * 2,
                        borderRect.Height);

                    // Check again for zero/negative width before creating brush
                    if (gradientRect.Width <= 0 || gradientRect.Height <= 0) return;


                    using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                       gradientRect,
                       borderColor,
                       borderEndColor,
                       LinearGradientMode.Horizontal))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { borderColor, borderEndColor, borderColor };
                        colorBlend.Positions = new float[] { 0f, 0.5f, 1.0f };

                        try // Added try/catch around potential GDI+ operation failure
                        {
                            gradientBrush.InterpolationColors = colorBlend;
                        }
                        catch (ArgumentException argEx)
                        {
                            // Log or handle error if ColorBlend is somehow invalid despite checks
                            System.Diagnostics.Debug.WriteLine($"ColorBlend Error: {argEx.Message}");
                            return; // Don't proceed with drawing if blend is invalid
                        }


                        float pixelOffset = gradientOffset * borderRect.Width;
                        gradientBrush.TranslateTransform(-pixelOffset, 0);
                        gradientBrush.WrapMode = WrapMode.Tile;

                        using (Pen borderPen = new Pen(gradientBrush, borderWidth))
                        {
                            e.Graphics.DrawPath(borderPen, borderPath);
                        }

                        // Reset transform is good practice, though maybe not strictly needed here
                        // as the brush is disposed immediately after.
                        gradientBrush.ResetTransform();
                    }
                }
            }
        }

        // --- PanelNav Custom Painting (Rounded Corners) ---
        //
        private int panelNavBorderRadius = 15; // Radius for the panelNav corners

        private void PanelNav_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null || panel.ClientRectangle.Width <= 0 || panel.ClientRectangle.Height <= 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Use the panelNavBorderRadius for the panel
            using (GraphicsPath path = CreateRoundedRectPath(panel.ClientRectangle, panelNavBorderRadius))
            {
                // Fill the panel background with its specified BackColor respecting the rounded corners
                using (Brush backBrush = new SolidBrush(panel.BackColor))
                {
                    // Clear the area first (optional, but can help prevent artifacts)
                    e.Graphics.Clear(this.TransparencyKey); // Use form's transparency key if panel sits directly on form
                    e.Graphics.FillPath(backBrush, path);
                }

                // Optional: Draw a border around the panel if desired
                // using (Pen panelBorderPen = new Pen(Color.Gray, 1)) // Example: 1px gray border
                // {
                //     // Adjust path slightly for border thickness if needed
                //     Rectangle borderBounds = panel.ClientRectangle;
                //     borderBounds.Width--; // Adjust for 1px pen
                //     borderBounds.Height--;
                //     using (GraphicsPath borderPath = CreateRoundedRectPath(borderBounds, panelNavBorderRadius))
                //     {
                //         e.Graphics.DrawPath(panelBorderPen, borderPath);
                //     }
                // }
            }
        }

        // --- Mouse Drag Logic (Assuming these exist from previous context) ---
        private Point dragStartPoint = Point.Empty;
        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private Panel panelNav;
        private PictureBox pictureBox1;
        private Panel panelMain;
        private Button buttonSettings;
        private Button buttonBlueprint;
        private System.Windows.Forms.Timer timer1;
        private Label lblSensorStatus;
        private Button buttonSetting;
        // -------------------------------------------------------------------

    } // End partial class Form1
} // End namespace Security