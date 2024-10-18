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

            V1.BezierStructure = V2.BezierStructure = this;

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

            for (int i = 0; i < Points.Length; i += 2)
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

        public void AdjustCurvePoints(Point V, int dx, int dy, Point originPoint)
        {
            if (V0.X == V3.X && V0.Y == V3.Y)
                return;

            bool movedV0 = V == V0;
            Point stationary = movedV0 ? V3 : V0;
            Point moved = V;

            double originalDx = moved.X - stationary.X;
            double originalDy = moved.Y - stationary.Y;

            double newDx = originalDx + dx;
            double newDy = originalDy + dy;

            double scale = Math.Sqrt(newDx * newDx + newDy * newDy) /
                           Math.Sqrt(originalDx * originalDx + originalDy * originalDy);

            double originalAngle = Math.Atan2(originalDy, originalDx);
            double newAngle = Math.Atan2(newDy, newDx);
            double rotation = newAngle - originalAngle;

            MovePoint(V1, stationary, scale, rotation, originPoint);
            MovePoint(V2, stationary, scale, rotation, originPoint);
        }

        private void MovePoint(Point point, Point reference, double scale, double rotation, Point originPoint)
        {
            double translatedX = point.X - reference.X;
            double translatedY = point.Y - reference.Y;

            translatedX *= scale;
            translatedY *= scale;

            double rotatedX = translatedX * Math.Cos(rotation) - translatedY * Math.Sin(rotation);
            double rotatedY = translatedX * Math.Sin(rotation) + translatedY * Math.Cos(rotation);

            int finalX = (int)Math.Round(rotatedX + reference.X);
            int finalY = (int)Math.Round(rotatedY + reference.Y);

            point.MoveLocationIndependent(finalX - point.X, finalY - point.Y);

            if ((reference == V0 && point == V1) || (reference == V3 && point == V2))
            {
                if (reference == V0 && V0.L1 != null && V0.L2 != null && V0 != originPoint && V1 != originPoint)
                {
                    V0.MoveLocation(0, 0, originPoint, V1);
                    V0.MoveLocation(0, 0, V2, V0.L1.P1);
                }
                else if (reference == V3 && V3.L1 != null && V3.L2 != null && V3 != originPoint && V2 != originPoint)
                {
                    V3.MoveLocation(0, 0, originPoint, V2);
                    V3.MoveLocation(0, 0, V1, V3.L2.P2);
                }
            }
        }
    }
}
