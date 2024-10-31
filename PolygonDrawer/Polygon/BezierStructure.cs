namespace PolygonDrawer
{
    internal class BezierStructure
    {
        private Point[] V = new Point[4];
        private Line[] L = new Line[3];

        public Point V0
        {
            get
            {
                return V[0];
            }

            private set { V[0] = value; }
        }

        public Point V1
        {
            get
            {
                return V[1];
            }

            private set { V[1] = value; }
        }

        public Point V2
        {
            get
            {
                return V[2];
            }

            private set { V[2] = value; }
        }

        public Point V3
        {
            get
            {
                return V[3];
            }

            private set { V[3] = value; }
        }
        public Line L0
        {
            get
            {
                return L[0];
            }

            private set { L[0] = value; }
        }

        public Line L1
        {
            get
            {
                return L[1];
            }

            private set { L[1] = value; }
        }

        public Line L2
        {
            get
            {
                return L[2];
            }

            private set { L[2] = value; }
        }


        public BezierStructure(Line? line)
        {
            if (line == null)
                return;

            V0 = line.P1;
            V3 = line.P2;

            Line prevLine = line.P1.L1, nextLine = line.P2.L2;

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

            for (int i = 0; i < V.Length; i += 2)
            {
                drawLineAction(g, (int)V[i].X, (int)V[i].Y, (int)V[(i + 1) % V.Length].X, (int)V[(i + 1) % V.Length].Y, null);
            }

            System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
            System.Drawing.Rectangle r1 = new System.Drawing.Rectangle((int)(V[1].X - Polygon.Eps), (int)(V[1].Y - Polygon.Eps), 2 * Polygon.Eps, 2 * Polygon.Eps);
            System.Drawing.Rectangle r2 = new System.Drawing.Rectangle((int)(V[2].X - Polygon.Eps), (int)(V[2].Y - Polygon.Eps), 2 * Polygon.Eps, 2 * Polygon.Eps);

            g.FillEllipse(brush, r1);
            g.FillEllipse(brush, r2);
        }

        public bool InBounds(double x, double y)
        {
            double minX = double.MaxValue, maxX = double.MinValue, minY = double.MaxValue, maxY = double.MinValue;

            foreach (var point in V)
            {
                minX = Math.Min(minX, point.X - Polygon.Eps);
                minY = Math.Min(minY, point.Y - Polygon.Eps);
                maxX = Math.Max(maxX, point.X + Polygon.Eps);
                maxY = Math.Max(maxY, point.Y + Polygon.Eps);
            }

            return minX <= x && minY <= y && maxX >= x && maxY >= y;
        }

        public void AdjustCurvePoints(Point movedPoint, double dx, double dy, Point originPoint)
        {
            if (V0.X == V3.X && V0.Y == V3.Y)
                return;

            bool movedV0 = movedPoint == V0;
            Point stationary = movedV0 ? V3 : V0;
            Point moved = movedPoint;

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
                if (reference == V0 && V0 != originPoint && V1 != originPoint)
                {
                    V0.MoveLocation(0, 0, originPoint, V1);
                    V0.MoveLocation(0, 0, V2, V0.L1.P1);
                }
                else if (reference == V3 && V3 != originPoint && V2 != originPoint)
                {
                    V3.MoveLocation(0, 0, originPoint, V2);
                    V3.MoveLocation(0, 0, V1, V3.L2.P2);
                }
            }
        }
    }
}
