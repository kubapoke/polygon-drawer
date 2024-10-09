using System.Diagnostics.Eventing.Reader;

namespace PolygonDrawer
{
    public partial class Form1 : Form
    {
        private bool CreatingNewPolygon = false, MovingPolygon = false, MovingPoint = false;
        private Polygon? Polygon = null;
        private int PrevMouseX, PrevMouseY;
        private Point? MovedPoint, InspectedPoint;
        private Line? InspectedLine;

        public Form1()
        {
            InitializeComponent();

            mainSplitContainer.Panel1.Paint += (sender, e) =>
            {
                if (Polygon == null)
                    return;

                var brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                var pen = new System.Drawing.Pen(System.Drawing.Color.Black);

                foreach (var point in Polygon.Points)
                {
                    var rectangle = new System.Drawing.Rectangle(point.X - Polygon.Eps, point.Y - Polygon.Eps, 2 * Polygon.Eps, 2 * Polygon.Eps);
                    e.Graphics.FillEllipse(brush, rectangle);
                }

                foreach (var line in Polygon.Lines)
                {
                    e.Graphics.DrawLine(pen, line.P1.X, line.P1.Y, line.P2.X, line.P2.Y);
                }
            };
        }

        private void adjustVertexToolStripMenu()
        {
            if(Polygon != null)
            {
                deleteVertexToolStripMenuItem.Enabled = Polygon.N > 3;
            }
        }

        private void adjustLineToolStripMenu()
        {
            if (Polygon != null)
            {
                
            }
        }

        private void newPolyButton_Click(object sender, EventArgs e)
        {
            Polygon = new Polygon();
            CreatingNewPolygon = true;
            mainSplitContainer.Panel1.Invalidate();
        }

        private void mainSplitContainer_Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CreatingNewPolygon && Polygon != null)
                {
                    if (Polygon.N >= 3 && Polygon.Points[0].InBounds(e.X, e.Y))
                    {
                        Polygon.Lines.Add(new Line(Polygon.Points[Polygon.N - 1], Polygon.Points[0]));

                        CreatingNewPolygon = false;
                    }
                    else
                    {
                        Polygon.Points.Add(new Point(e.Location.X, e.Location.Y));
                        if (Polygon.N >= 2)
                            Polygon.Lines.Add(new Line(Polygon.Points[Polygon.N - 2], Polygon.Points[Polygon.N - 1]));
                    }

                    mainSplitContainer.Panel1.Invalidate();
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

                        if(InspectedLine != null)
                        {
                            adjustLineToolStripMenu();
                            lineContextMenuStrip.Show(Cursor.Position);
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
                    point.X += xMove;
                    point.Y += yMove;
                }

                mainSplitContainer.Panel1.Invalidate();

                PrevMouseX = e.X;
                PrevMouseY = e.Y;
            }
            else if (MovingPoint)
            {
                int xMove = e.X - PrevMouseX, yMove = e.Y - PrevMouseY;

                MovedPoint!.X += xMove;
                MovedPoint!.Y += yMove;

                mainSplitContainer.Panel1.Invalidate();

                PrevMouseX = e.X;
                PrevMouseY = e.Y;
            }
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
            mainSplitContainer.Panel1.Invalidate();
        }
    }
}
