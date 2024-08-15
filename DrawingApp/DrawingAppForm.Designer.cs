namespace DrawingApp
{
  partial class DrawingAppForm
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingAppForm));
            tsButton = new ToolStrip();
            btSelect = new ToolStripButton();
            btLine = new ToolStripButton();
            btCircle = new ToolStripButton();
            btPolygon = new ToolStripButton();
            bt_Polyline = new ToolStripButton();
            bt_Delete = new ToolStripButton();
            bt_Copy = new ToolStripButton();
            bt_Paste = new ToolStripButton();
            pbCanvas = new PictureBox();
            tsButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbCanvas).BeginInit();
            SuspendLayout();
            // 
            // tsButton
            // 
            tsButton.BackColor = SystemColors.GradientInactiveCaption;
            tsButton.ImageScalingSize = new Size(60, 60);
            tsButton.Items.AddRange(new ToolStripItem[] { btSelect, btLine, btCircle, btPolygon, bt_Polyline, bt_Delete, bt_Copy, bt_Paste });
            tsButton.Location = new Point(0, 0);
            tsButton.Name = "tsButton";
            tsButton.Size = new Size(974, 67);
            tsButton.TabIndex = 0;
            tsButton.Text = "toolStrip1";
            // 
            // btSelect
            // 
            btSelect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btSelect.Image = (Image)resources.GetObject("btSelect.Image");
            btSelect.ImageTransparentColor = Color.Magenta;
            btSelect.Name = "btSelect";
            btSelect.Size = new Size(64, 64);
            btSelect.Text = "Select";
            btSelect.Click += btSelect_Click;
            // 
            // btLine
            // 
            btLine.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btLine.Image = (Image)resources.GetObject("btLine.Image");
            btLine.ImageTransparentColor = Color.Magenta;
            btLine.Name = "btLine";
            btLine.Size = new Size(64, 64);
            btLine.Text = "Line";
            btLine.Click += btLine_Click;
            // 
            // btCircle
            // 
            btCircle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btCircle.Image = (Image)resources.GetObject("btCircle.Image");
            btCircle.ImageTransparentColor = Color.Magenta;
            btCircle.Name = "btCircle";
            btCircle.Size = new Size(64, 64);
            btCircle.Text = "Circle";
            btCircle.Click += btCircle_Click;
            // 
            // btPolygon
            // 
            btPolygon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btPolygon.Image = (Image)resources.GetObject("btPolygon.Image");
            btPolygon.ImageTransparentColor = Color.Magenta;
            btPolygon.Name = "btPolygon";
            btPolygon.Size = new Size(64, 64);
            btPolygon.Text = "polygon";
            btPolygon.Click += btPolygon_Click;
            // 
            // bt_Polyline
            // 
            bt_Polyline.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bt_Polyline.Image = (Image)resources.GetObject("bt_Polyline.Image");
            bt_Polyline.ImageTransparentColor = Color.Magenta;
            bt_Polyline.Name = "bt_Polyline";
            bt_Polyline.Size = new Size(64, 64);
            bt_Polyline.Text = "Polyline";
            bt_Polyline.Click += bt_Polyline_Click;
            // 
            // bt_Delete
            // 
            bt_Delete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bt_Delete.Image = (Image)resources.GetObject("bt_Delete.Image");
            bt_Delete.ImageTransparentColor = Color.Magenta;
            bt_Delete.Name = "bt_Delete";
            bt_Delete.Size = new Size(64, 64);
            bt_Delete.Text = "Delete";
            bt_Delete.Click += bt_Delete_Click;
            // 
            // bt_Copy
            // 
            bt_Copy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bt_Copy.Image = (Image)resources.GetObject("bt_Copy.Image");
            bt_Copy.ImageTransparentColor = Color.Magenta;
            bt_Copy.Name = "bt_Copy";
            bt_Copy.Size = new Size(64, 64);
            bt_Copy.Text = "Copy";
            bt_Copy.Click += bt_Copy_Click;
            // 
            // bt_Paste
            // 
            bt_Paste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bt_Paste.Image = (Image)resources.GetObject("bt_Paste.Image");
            bt_Paste.ImageTransparentColor = Color.Magenta;
            bt_Paste.Name = "bt_Paste";
            bt_Paste.Size = new Size(64, 64);
            bt_Paste.Text = "Paste";
            bt_Paste.Click += bt_Paste_Click;
            // 
            // pbCanvas
            // 
            pbCanvas.BackColor = Color.Silver;
            pbCanvas.Dock = DockStyle.Fill;
            pbCanvas.Location = new Point(0, 67);
            pbCanvas.Margin = new Padding(3, 4, 3, 4);
            pbCanvas.Name = "pbCanvas";
            pbCanvas.Size = new Size(974, 536);
            pbCanvas.TabIndex = 1;
            pbCanvas.TabStop = false;
            pbCanvas.Paint += pbCanvas_Paint;
            pbCanvas.MouseDown += pbCanvas_MouseDown;
            pbCanvas.MouseMove += pbCanvas_MouseMove;
            pbCanvas.MouseUp += pbCanvas_MouseUp;
            // 
            // DrawingAppForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(974, 603);
            Controls.Add(pbCanvas);
            Controls.Add(tsButton);
            Margin = new Padding(3, 4, 3, 4);
            Name = "DrawingAppForm";
            Text = "Drawing App";
            KeyDown += DrawingAppForm_KeyDown;
            KeyUp += DrawingAppForm_KeyUp;
            tsButton.ResumeLayout(false);
            tsButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbCanvas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip tsButton;
    private PictureBox pbCanvas;
    private ToolStripButton btCircle;
        private ToolStripButton btLine;
        private ToolStripButton btPolygon;
        private ToolStripButton bt_Polyline;
        private ToolStripButton btSelect;
        private ToolStripButton bt_Delete;
        private ToolStripButton bt_Copy;
        private ToolStripButton bt_Paste;
    }
}
