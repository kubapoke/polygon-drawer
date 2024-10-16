using System.Reflection;

namespace PolygonDrawer
{
    public partial class MainWindowForm : Form
    {
        internal static Polygon? Polygon = null;

        private bool CreatingNewPolygon = false, MovingPolygon = false, MovingPoint = false, CaptionsEnabled = true;
        private int PrevMouseX, PrevMouseY;
        private Point? MovedPoint, InspectedPoint, SuggestedPoint;
        private Line? InspectedLine;
        private Action<Graphics, int, int, int, int, System.Drawing.Color?> DrawLineAction = Drawer.BresenhamDrawLine;


        public MainWindowForm()
        {
            InitializeComponent();

            mainSplitContainer.Panel1.Paint += (sender, e) =>
            {
                if (Polygon == null)
                    return;

                DrawLineAction = libraryRadioButton.Checked ? Drawer.LibraryDrawLine : Drawer.BresenhamDrawLine;

                Polygon.Draw(e.Graphics, DrawLineAction, this.PointToClient(Cursor.Position), CaptionsEnabled);
            };

            typeof(Panel).InvokeMember("DoubleBuffered",                                    // taken from https://stackoverflow.com/questions/8046560/how-to-stop-flickering-c-sharp-winforms
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, mainSplitContainer.Panel1, new object[] { true });
        }

        private void adjustVertexToolStripMenu()
        {
            if (Polygon != null)
            {
                deleteVertexToolStripMenuItem.Enabled = Polygon.N > 3;
            }
        }

        private void adjustLineToolStripMenu()
        {
            if (Polygon != null && InspectedLine != null)
            {
                bool hasVerticalNeighbors = false;
                bool hasHorizontalNeighbors = false;

                if (InspectedLine.P1.L1 != null && InspectedLine.P1.L1.State == Line.LineState.Vertical)
                    hasVerticalNeighbors = true;
                if (InspectedLine.P2.L2 != null && InspectedLine.P2.L2.State == Line.LineState.Vertical)
                    hasVerticalNeighbors = true;
                if (InspectedLine.P1.L1 != null && InspectedLine.P1.L1.State == Line.LineState.Horizontal)
                    hasHorizontalNeighbors = true;
                if (InspectedLine.P2.L2 != null && InspectedLine.P2.L2.State == Line.LineState.Horizontal)
                    hasHorizontalNeighbors = true;

                forceVerticalToolStripMenuItem.Enabled = InspectedLine.State != Line.LineState.Vertical && !hasVerticalNeighbors;
                forceHorizontalToolStripMenuItem.Enabled = InspectedLine.State != Line.LineState.Horizontal && !hasHorizontalNeighbors;
                currentLengthToolStripMenuItem.Enabled = InspectedLine.State != Line.LineState.ForcedLength;
                setBezierCurveToolStripMenuItem.Enabled = InspectedLine.State != Line.LineState.Bezier;
                removeBoundsToolStripMenuItem.Enabled = !InspectedLine.IsDefault();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // overriden for creating shortcuts, for testing and fun
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    Polygon.ChangeStateOfAllLines(Line.LineState.ForcedLength);
                }

                redrawPolygon();

                return true;
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    Polygon.ChangeStateOfAllLines(Line.LineState.None);
                }

                redrawPolygon();

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void redrawPolygon()
        {
            mainSplitContainer.Panel1.Invalidate();
        }

        private void newPolyButton_Click(object sender, EventArgs e)
        {
            Polygon = new Polygon();
            CreatingNewPolygon = true;
            redrawPolygon();
        }

        private void mainSplitContainer_Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CreatingNewPolygon && Polygon != null)
                {
                    CreatingNewPolygon = Polygon.AddCreationPoint(e.X, e.Y);

                    redrawPolygon();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    InspectedPoint = Polygon.GetPointAtLocation(e.X, e.Y);

                    if (InspectedPoint != null)
                    {
                        adjustVertexToolStripMenu();
                        vertexContextMenuStrip.Show(Cursor.Position);
                    }
                    else
                    {
                        InspectedLine = Polygon.GetLineAtLocation(e.X, e.Y);

                        if (InspectedLine != null)
                        {
                            adjustLineToolStripMenu();
                            lineContextMenuStrip.Show(Cursor.Position);
                            SuggestedPoint = new Point(e.X, e.Y);
                        }
                    }
                }
            }
        }

        private void mainSplitContainer_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Polygon != null && MovingPolygon)
            {
                int xMove = e.X - PrevMouseX, yMove = e.Y - PrevMouseY;

                Polygon.MovePolygon(xMove, yMove);
            }
            else if (MovedPoint != null && MovingPoint)
            {
                int xMove = e.X - PrevMouseX, yMove = e.Y - PrevMouseY;

                MovedPoint.MoveLocation(xMove, yMove, MovedPoint);
            }

            redrawPolygon();

            PrevMouseX = e.X;
            PrevMouseY = e.Y;
        }

        private void mainSplitContainer_Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MovingPolygon = false;
                MovingPoint = false;
            }
        }

        private void mainSplitContainer_Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    MovedPoint = Polygon.GetPointAtLocation(e.X, e.Y);

                    if (MovedPoint != null)
                    {
                        PrevMouseX = e.X; PrevMouseY = e.Y;
                        MovingPoint = true;
                    }
                    else if (Polygon.InBounds(e.X, e.Y))
                    {
                        PrevMouseX = e.X; PrevMouseY = e.Y;
                        MovingPolygon = true;
                    }
                }
            }
        }

        private void deleteVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Polygon!.DeletePoint(InspectedPoint!);
            redrawPolygon();
        }

        private void addPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Polygon!.AddPoint(InspectedLine!, SuggestedPoint!);
            redrawPolygon();
        }

        private void libraryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (libraryRadioButton.Checked)
            {
                bresenhamRadioButton.Checked = false;
                redrawPolygon();
            }
        }

        private void bresenhamRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (bresenhamRadioButton.Checked)
            {
                libraryRadioButton.Checked = false;
                redrawPolygon();
            }
        }

        private void forceVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.Vertical);
                redrawPolygon();
            }
        }

        private void forceHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.Horizontal);
                redrawPolygon();
            }
        }

        private void setBezierCurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.Bezier);
                redrawPolygon();
            }
        }

        private void removeBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.None);
                redrawPolygon();
            }
        }

        private void captionButton_Click(object sender, EventArgs e)
        {
            CaptionsEnabled = !CaptionsEnabled;

            captionButton.Text = CaptionsEnabled ? "Captions: Enabled" : "Captions: Disabled";

            redrawPolygon();
        }

        private void currentLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.ForcedLength);
                redrawPolygon();
            }
        }

        private void setLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                var inputForm = new LengthInputForm(InspectedLine.Length);

                if (inputForm.ShowDialog() == DialogResult.OK && inputForm.Result != null)
                {
                    InspectedLine.SetWantedLength((double)inputForm.Result);

                    InspectedLine.ChangeState(Line.LineState.ForcedLength);
                    redrawPolygon();
                }
            }
        }
    }
}
