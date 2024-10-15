using System.Reflection;

namespace PolygonDrawer
{
    public partial class Form1 : Form
    {
        internal static Polygon? Polygon = null;

        private bool CreatingNewPolygon = false, MovingPolygon = false, MovingPoint = false;
        private int PrevMouseX, PrevMouseY;
        private Point? MovedPoint, InspectedPoint, SuggestedPoint;
        private Line? InspectedLine;
        private Action<Graphics, int, int, int, int, System.Drawing.Color?> DrawLineAction = LineDrawer.BresenhamDrawLine;
      

        public Form1()
        {
            InitializeComponent();

            mainSplitContainer.Panel1.Paint += (sender, e) =>
            {
                if (Polygon == null)
                    return;                

                DrawLineAction = libraryRadioButton.Checked ? LineDrawer.LibraryDrawLine : LineDrawer.BresenhamDrawLine;

                Polygon.Draw(e.Graphics, CreatingNewPolygon, DrawLineAction, this.PointToClient(Cursor.Position));
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
                forceLengthToolStripMenuItem.Enabled = InspectedLine.State != Line.LineState.FixedLength;
                setBezierCurveToolStripMenuItem.Enabled = false; // InspectedLine.IsDefault();
                removeBoundsToolStripMenuItem.Enabled = !InspectedLine.IsDefault();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // overriden for creating shortcuts, for testing and fun
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    foreach (var line in Polygon.Lines)
                    {
                        line.ChangeState(Line.LineState.FixedLength);
                    }
                }

                return true;
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    foreach (var line in Polygon.Lines)
                    {
                        line.ChangeState(Line.LineState.None);
                    }
                }

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
                    if (Polygon.N >= 3 && Polygon.Points[0].InBounds(e.X, e.Y))
                    {
                        Line line = new Line(Polygon.Points[Polygon.N - 1], Polygon.Points[0]);
                        Polygon.Points[Polygon.N - 1].L2 = Polygon.Points[0].L1 = line;
                        Polygon.Lines.Add(line);

                        CreatingNewPolygon = false;
                    }
                    else
                    {
                        Polygon.Points.Add(new Point(e.Location.X, e.Location.Y));
                        if (Polygon.N >= 2)
                        {
                            Line line = new Line(Polygon.Points[Polygon.N - 2], Polygon.Points[Polygon.N - 1]);
                            Polygon.Points[Polygon.N - 2].L2 = Polygon.Points[Polygon.N - 1].L1 = line;
                            Polygon.Lines.Add(line);
                        }

                    }

                    redrawPolygon();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    InspectedPoint = Polygon.Points.Find(p => p.InBounds(e.X, e.Y));

                    if (InspectedPoint != null)
                    {
                        adjustVertexToolStripMenu();
                        vertexContextMenuStrip.Show(Cursor.Position);
                    }
                    else
                    {
                        InspectedLine = Polygon.Lines.Find(l => l.InBounds(e.X, e.Y));

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
            if (MovingPolygon)
            {
                int xMove = e.X - PrevMouseX, yMove = e.Y - PrevMouseY;

                foreach (var point in Polygon!.Points)
                {
                    point.MoveLocationIndependent(xMove, yMove);
                }
            }
            else if (MovingPoint)
            {
                int xMove = e.X - PrevMouseX, yMove = e.Y - PrevMouseY;

                MovedPoint!.MoveLocation(xMove, yMove, MovedPoint);
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
                    MovedPoint = Polygon.Points.Find(p => p.InBounds(e.X, e.Y));

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
            }
        }

        private void forceHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.Horizontal);
            }
        }

        private void forceLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.FixedLength);
            }
        }

        private void setBezierCurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.Bezier);
            }
        }

        private void removeBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                InspectedLine.ChangeState(Line.LineState.None);
            }
        }
    }
}
