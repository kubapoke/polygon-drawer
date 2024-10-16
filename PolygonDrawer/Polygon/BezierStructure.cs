namespace PolygonDrawer
{
    internal class BezierStructure
    {
        private Point[] Points = new Point[4];
        private Line[] Lines = new Line[3];

        public Point V0
        {
            get
            {
                return Points[0];
            }

            private set { Points[0] = value; }
        }

        public Point V1
        {
            get
            {
                return Points[1];
            }

            private set { Points[1] = value; }
        }

        public Point V2
        {
            get
            {
                return Points[2];
            }

            private set { Points[2] = value; }
        }

        public Point V3
        {
            get
            {
                return Points[3];
            }

            private set { Points[3] = value; }
        }
        public Line L0
        {
            get
            {
                return Lines[0];
            }

            private set { Lines[0] = value; }
        }

        public Line L1
        {
            get
            {
                return Lines[1];
            }

            private set { Lines[1] = value; }
        }

        public Line L2
        {
            get
            {
                return Lines[2];
            }

            private set { Lines[2] = value; }
        }


        public BezierStructure(Line line)
        {
            V0 = line.P1;
            V3 = line.P2;

            Line prevLine = line.P1.L1!, nextLine = line.P2.L2!;

            V1 = new Point(2 * line.P1.X - prevLine.P1.X, 2 * line.P1.Y - prevLine.P1.Y);
            V2 = new Point(2 * line.P2.X - nextLine.P2.X, 2 * line.P2.Y - nextLine.P2.Y);

            V1.ChangeState(Point.PointState.Bezier);
            V2.ChangeState(Point.PointState.Bezier);

            L0 = new Line(V0, V1);
            L1 = new Line(V1, V2);
            L2 = new Line(V2, V3);

            V0.L2 = V1.L1 = L0;
            V1.L2 = V2.L1 = L1;
            V2.L2 = V3.L1 = L2;
        }

        public void Draw(Graphics g, Action<Graphics, int, int, int, int, Color?> drawLineAction)
        {
            Drawer.DrawBezierCurve(g, V0, V1, V2, V3);

            for (int i = 0; i < Points.Length; i+=2)
            {
                drawLineAction(g, Points[i].X, Points[i].Y, Points[(i + 1) % Points.Length].X, Points[(i + 1) % Points.Length].Y, null);
            }

            System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
            System.Drawing.Rectangle r1 = new System.Drawing.Rectangle(Points[1].X - Polygon.Eps, Points[1].Y - Polygon.Eps, 2 * Polygon.Eps, 2 * Polygon.Eps);
            System.Drawing.Rectangle r2 = new System.Drawing.Rectangle(Points[2].X - Polygon.Eps, Points[2].Y - Polygon.Eps, 2 * Polygon.Eps, 2 * Polygon.Eps);

            g.FillEllipse(brush, r1);
            g.FillEllipse(brush, r2);
        }

        public bool InBounds(int x, int y)
        {
            int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;

            foreach (var point in Points)
            {
                minX = Math.Min(minX, point.X - Polygon.Eps);
                minY = Math.Min(minY, point.Y - Polygon.Eps);
                maxX = Math.Max(maxX, point.X + Polygon.Eps);
                maxY = Math.Max(maxY, point.Y + Polygon.Eps);
            }

            return minX <= x && minY <= y && maxX >= x && maxY >= y;
        }
    }
}
