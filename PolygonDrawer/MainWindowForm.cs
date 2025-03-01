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

        private readonly Dictionary<string, string> Instructions = new Dictionary<string, string>()
        {
            { "base", "Click the \"New Polygon\" button to delete the current shape and start creating a new one.\n\nHold and drag a vertex with your left mouse button to move it, or hold and drag the polygon to move its entirety.\n\n" +
                "Right click a vertex or an edge to open a context menu with more options.\n\nYou can disable and re-enable captions with the \"Captions\" button, as well as swap the line drawing algorithm with the provided radio button" },
            { "drawing", "Click anywhere within the drawing area to create a new polygon vertex (connected to the previous one you've created).\n\nOnce you have drawn at least three vertices, connect the last vertex to the first one, in order to " +
                "finish drawing." },
            { "vertex", "Delete point - remove the point, connecting the adjacent vertices with an unconstrained line.\n\nThe following continuity options only affect vertices which are a staring/ending point of a Bezier curve:\n\n" +
                "Set G0 continuity - sets G0 continuity for the vertex.\n\n" +
                "Set G1 continuity - sets G1 continuity for the vertex.\n\n" +
                "Set C1 continuity - sets C1 continuity for the vertex." },
            { "edge", "Add point - adds a vertex at the selected position. Removes constraints from adjacent edges.\n\n" +
                "Make vertical - forces the line to be vertical.\n\n" +
                "Make horizontal - forces the line to be horizontal.\n\n" +
                "Force length - forces the edge's length to either to its current length or a specified length, depending on the suboption chosen.\n\n" +
                "Create Bezier curve - transforms the edge into a Bezier curve.\n\n" +
                "Remove restrictions - remove any restrictions from the edge." }

        };


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

            ChangeText("base");

            // create an example polygon

            {
                Polygon = new Polygon();

                Polygon.AddCreationPoint(150, 350);
                Polygon.AddCreationPoint(450, 300);
                Polygon.AddCreationPoint(430, 202);
                Polygon.AddCreationPoint(225, 100);
                Polygon.AddCreationPoint(150, 200);
                Polygon.AddCreationPoint(150, 350);

                Line l;
                Point p;

                l = Polygon.GetLineAtLocation(150, 275)!;
                l.ChangeState(Line.LineState.Vertical);

                l = Polygon.GetLineAtLocation(437, 250)!;
                l.ChangeState(Line.LineState.FixedLength);

                l = Polygon.GetLineAtLocation(327, 151)!;
                l.ChangeState(Line.LineState.Bezier);

                p = Polygon.GetPointAtLocation(430, 202)!;
                p.ChangeState(Point.PointState.G0Continuous);

                p = Polygon.GetPointAtLocation(225, 100)!;
                p.ChangeState(Point.PointState.G1Continuous);

                p = Polygon.GetPointAtLocation(250, 67)!;
                p.MoveLocation(25, -33);

                p = Polygon.GetPointAtLocation(423, 169)!;
                p.MoveLocation(75, -50);

                Polygon.MovePolygon(0, 25);

                redrawPolygon();
            }
        }

        private void adjustVertexToolStripMenu()
        {
            if (Polygon != null && InspectedPoint != null)
            {
                deleteVertexToolStripMenuItem.Enabled = InspectedPoint.State != Point.PointState.Bezier && Polygon.N > 3;
                setG0ContinuityToolStripMenuItem.Enabled = InspectedPoint.ControlsContinuity && InspectedPoint.State != Point.PointState.G0Continuous;
                setG1ContinuityToolStripMenuItem.Enabled = InspectedPoint.ControlsContinuity && InspectedPoint.State != Point.PointState.G1Continuous;
                setC1ContinuityToolStripMenuItem.Enabled = InspectedPoint.ControlsContinuity && InspectedPoint.State != Point.PointState.C1Continuous;
            }
        }

        private void adjustLineToolStripMenu()
        {
            if (Polygon != null && InspectedLine != null)
            {
                bool hasVerticalNeighbors = false;
                bool hasHorizontalNeighbors = false;
                bool isBezierLine = false;

                if (InspectedLine.P1.L1.State == Line.LineState.Vertical)
                    hasVerticalNeighbors = true;
                if (InspectedLine.P2.L2.State == Line.LineState.Vertical)
                    hasVerticalNeighbors = true;
                if (InspectedLine.P1.L1.State == Line.LineState.Horizontal)
                    hasHorizontalNeighbors = true;
                if (InspectedLine.P2.L2.State == Line.LineState.Horizontal)
                    hasHorizontalNeighbors = true;
                isBezierLine = InspectedLine.State == Line.LineState.Bezier;

                addPointToolStripMenuItem.Enabled = !isBezierLine;
                forceVerticalToolStripMenuItem.Enabled = InspectedLine.State != Line.LineState.Vertical && !hasVerticalNeighbors && !isBezierLine;
                forceHorizontalToolStripMenuItem.Enabled = InspectedLine.State != Line.LineState.Horizontal && !hasHorizontalNeighbors && !isBezierLine;
                currentLengthToolStripMenuItem.Enabled = InspectedLine.State != Line.LineState.FixedLength && !isBezierLine;
                setLengthToolStripMenuItem.Enabled = !isBezierLine;
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
                    Polygon.ChangeStateOfAllLines(Line.LineState.None);
                    Polygon.ChangeStateOfAllLines(Line.LineState.FixedLength);
                }

                redrawPolygon();

                return true;
            }
            else if (keyData == (Keys.Shift | Keys.Control | Keys.F))
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    Polygon.ChangeStateOfAllLines(Line.LineState.None);
                    Polygon.ChangeStateOfAllLines(Line.LineState.FixedLength, 100);
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
            else if (keyData == (Keys.Control | Keys.B))
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    Polygon.ChangeStateOfAllLines(Line.LineState.None);
                    Polygon.ChangeStateOfAllLines(Line.LineState.Bezier);
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
            ChangeText("drawing");
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

                    if (CreatingNewPolygon == false)
                        ChangeText("base");
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!CreatingNewPolygon && Polygon != null)
                {
                    InspectedPoint = Polygon.GetRealPointAtLocation(e.X, e.Y);

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

                MovedPoint.MoveLocation(e.X - MovedPoint.X, e.Y - MovedPoint.Y);
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
                InspectedLine.ChangeState(Line.LineState.FixedLength);
                redrawPolygon();
            }
        }

        private void setLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedLine != null)
            {
                var inputForm = new LengthInputForm(InspectedLine.State == Line.LineState.FixedLength ? InspectedLine.WantedLength : InspectedLine.Length, x => InspectedLine.ValidateSetLength(x));

                if (inputForm.ShowDialog() == DialogResult.OK && inputForm.Result != null)
                {
                    InspectedLine.ChangeState(Line.LineState.FixedLength);
                    InspectedLine.SetWantedLength((double)inputForm.Result);
                    InspectedLine.Touch();

                    redrawPolygon();
                }
            }
        }

        private void setG0ContinuityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedPoint != null)
            {
                InspectedPoint.ChangeState(Point.PointState.G0Continuous);
                redrawPolygon();
            }
        }

        private void setG1ContinuityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedPoint != null)
            {
                InspectedPoint.ChangeState(Point.PointState.G1Continuous);
                redrawPolygon();
            }
        }

        private void setC1ContinuityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InspectedPoint != null)
            {
                InspectedPoint.ChangeState(Point.PointState.C1Continuous);
                redrawPolygon();
            }
        }

        private void ChangeText(string key)
        {
            if (Instructions.ContainsKey(key))
            {
                instructionsTextBox.Text = Instructions[key];
            }
        }

        private void vertexContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ChangeText("vertex");
        }

        private void vertexContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ChangeText("base");
        }

        private void lineContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ChangeText("edge");
        }

        private void lineContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ChangeText("base");
        }
    }
}
