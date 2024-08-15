namespace DrawingApp
{
    public partial class DrawingAppForm : Form
    {
        private DrawingManager _dManager;
        public DrawingAppForm()
        {
            InitializeComponent();
            _dManager = new DrawingManager(this);
        }

        public PictureBox Canvas { get { return pbCanvas; } }

        private void btCircle_Click(object sender, EventArgs e)
        {
            _dManager.SetMode(Mode.Circle);
        }

        private void pbCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            _dManager.MouseUp(e);
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            _dManager.Draw(e.Graphics);
        }

        private void DrawingAppForm_KeyDown(object sender, KeyEventArgs e)
        {
            _dManager.KeyDown(e.KeyCode);
        }

        private void btLine_Click(object sender, EventArgs e)
        {
            _dManager.SetMode(Mode.Line);

        }

        private void pbCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            _dManager.MouseMove(e.Location);

        }

        private void btPolygon_Click(object sender, EventArgs e)
        {
            _dManager.SetMode(Mode.Polygon);
        }

        private void bt_Polyline_Click(object sender, EventArgs e)
        {
            _dManager.SetMode(Mode.Polyline);
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            _dManager.SetModeToNone();
        }

        private void DrawingAppForm_KeyUp(object sender, KeyEventArgs e)
        {
            _dManager.KeyUp(e.KeyCode);
        }

        private void pbCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            _dManager.MouseDown(e);
        }

        private void bt_Delete_Click(object sender, EventArgs e)
        {
            _dManager.DeleteSelectedShapes();
        }

        private void bt_Copy_Click(object sender, EventArgs e)
        {
            _dManager.CopySelectedShapes();
        }

        private void bt_Paste_Click(object sender, EventArgs e)
        {
            _dManager.PasteCopiedShapes();
        }
    }
}
