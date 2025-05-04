using System; // Required for EventArgs
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Security
{
    partial class FormMain
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
            buttonSettings = new Button();
            buttonDesigner = new Button();
            buttonRemoveCam = new Button();
            lblSensorStatus = new Label();
            pictureBox1 = new PictureBox();
            panelMain = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            pbExit = new PictureBox();
            panelNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbExit).BeginInit();
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
            panelNav.Controls.Add(buttonSettings);
            panelNav.Controls.Add(buttonDesigner);
            panelNav.Controls.Add(buttonRemoveCam);
            panelNav.Controls.Add(lblSensorStatus);
            panelNav.Location = new Point(16, 5);
            panelNav.Name = "panelNav";
            panelNav.Size = new Size(213, 682);
            panelNav.TabIndex = 2;
            panelNav.Paint += PanelNav_Paint;
            // 
            // buttonSettings
            // 
            buttonSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonSettings.BackColor = Color.Transparent;
            buttonSettings.FlatStyle = FlatStyle.Flat;
            buttonSettings.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonSettings.ForeColor = SystemColors.ButtonHighlight;
            buttonSettings.Location = new Point(17, 608);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(180, 38);
            buttonSettings.TabIndex = 9;
            buttonSettings.Text = "SETTINGS";
            buttonSettings.UseVisualStyleBackColor = false;
            buttonSettings.Click += buttonSetting_Click;
            // 
            // buttonDesigner
            // 
            buttonDesigner.BackColor = Color.Transparent;
            buttonDesigner.FlatStyle = FlatStyle.Flat;
            buttonDesigner.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonDesigner.ForeColor = SystemColors.ButtonHighlight;
            buttonDesigner.Location = new Point(17, 124);
            buttonDesigner.Name = "buttonDesigner";
            buttonDesigner.Size = new Size(180, 38);
            buttonDesigner.TabIndex = 8;
            buttonDesigner.Text = "DESIGNER";
            buttonDesigner.UseVisualStyleBackColor = false;
            buttonDesigner.Click += buttonSettings_Click;
            // 
            // buttonRemoveCam
            // 
            buttonRemoveCam.BackColor = Color.Transparent;
            buttonRemoveCam.FlatStyle = FlatStyle.Flat;
            buttonRemoveCam.Font = new Font("Tempus Sans ITC", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonRemoveCam.ForeColor = SystemColors.ButtonHighlight;
            buttonRemoveCam.Location = new Point(17, 65);
            buttonRemoveCam.Name = "buttonRemoveCam";
            buttonRemoveCam.Size = new Size(180, 38);
            buttonRemoveCam.TabIndex = 7;
            buttonRemoveCam.Text = "DASHBOARD";
            buttonRemoveCam.UseVisualStyleBackColor = false;
            buttonRemoveCam.Click += buttonBlueprint_Click;
            // 
            // lblSensorStatus
            // 
            lblSensorStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSensorStatus.AutoSize = true;
            lblSensorStatus.Location = new Point(28, 659);
            lblSensorStatus.Name = "lblSensorStatus";
            lblSensorStatus.Size = new Size(38, 15);
            lblSensorStatus.TabIndex = 2;
            lblSensorStatus.Text = "label1";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(64, 64, 64);
            pictureBox1.Image = Properties.Resources.Menu1;
            pictureBox1.Location = new Point(9, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(45, 45);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // panelMain
            // 
            panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelMain.AutoSize = true;
            panelMain.Location = new Point(76, 5);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1223, 679);
            panelMain.TabIndex = 7;
            // 
            // pbExit
            // 
            pbExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbExit.BackColor = Color.FromArgb(64, 64, 64);
            pbExit.Image = Properties.Resources.exit;
            pbExit.Location = new Point(1261, 14);
            pbExit.Name = "pbExit";
            pbExit.Size = new Size(37, 35);
            pbExit.SizeMode = PictureBoxSizeMode.StretchImage;
            pbExit.TabIndex = 8;
            pbExit.TabStop = false;
            pbExit.Click += pbExit_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1312, 690);
            Controls.Add(pbExit);
            Controls.Add(pictureBox1);
            Controls.Add(panelNav);
            Controls.Add(panelMain);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormMain";
            Opacity = 0.9D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            DoubleClick += Form1_DoubleClick;
            MouseDoubleClick += Form1_MouseDoubleClick;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;
            panelNav.ResumeLayout(false);
            panelNav.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbExit).EndInit();
            ResumeLayout(false);
            PerformLayout();
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

        // --- PanelNav Custom Painting (Rounded Corners & Gradient with Opacity) ---
        //
        private int panelNavBorderRadius = 15; // Radius for the panelNav corners (Set to 0 if no rounding needed)

        private void PanelNav_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null || panel.ClientRectangle.Width <= 1 || panel.ClientRectangle.Height <= 1)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            // --- Define Opacity (0-255) ---
            int alpha = 128; // 50% Opacity

            // --- Define Gradient Colors WITH Alpha ---
            Color startColor = Color.FromArgb(alpha, Color.FromArgb(64, 64, 64)); // Apply alpha
            Color endColor = Color.FromArgb(alpha, Color.FromArgb(55, 55, 55));    // Apply alpha

            Rectangle gradientRect = panel.ClientRectangle;

            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                gradientRect,
                startColor, // Use color with alpha
                endColor,   // Use color with alpha
                LinearGradientMode.ForwardDiagonal))
            {
                if (panelNavBorderRadius > 0)
                {
                    using (GraphicsPath path = CreateRoundedRectPath(gradientRect, panelNavBorderRadius))
                    {
                        // Fill rounded path with the semi-transparent gradient
                        e.Graphics.FillPath(gradientBrush, path);
                        // Optional border would go here
                    }
                }
                else // No rounded corners
                {
                    // Fill entire rectangle with the semi-transparent gradient
                    e.Graphics.FillRectangle(gradientBrush, gradientRect);
                    // Optional border would go here
                }
            }

            // CHILD CONTROLS (Buttons, Labels inside panelNav) WILL STILL BE OPAQUE.
            // This transparency only applies to the panel's own background drawing.
        }

        // --- Mouse Drag Logic (Assuming these exist from previous context) ---
        private Point dragStartPoint = Point.Empty;
        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private Panel panelNav;
        private PictureBox pictureBox1;
        private Panel panelMain;
        private System.Windows.Forms.Timer timer1;
        private Label lblSensorStatus;
        private PictureBox pbExit;
        private Button buttonDesigner;
        private Button buttonRemoveCam;
        private Button buttonSettings;
        // -------------------------------------------------------------------

    } // End partial class Form1
} // End namespace Security