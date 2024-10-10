namespace PolygonDrawer
{
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Line? L1 { get; set; }
        public Line? L2 { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Distance(int x, int y)
        {
            return Math.Max(Math.Abs(x - this.X), Math.Abs(y - this.Y));
        }

        public bool InBounds(int x, int y)
        {
            return Distance(x, y) <= Polygon.Eps;
        }

        public void SetLocation(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void MoveLocation(int dx, int dy, Point? originPoint = null, Point? prevPoint = null)
        {
            if (this == originPoint && prevPoint != null)
                return;

            this.X += dx;
            this.Y += dy;

            if (L1 != null)
            {
                switch (L1.State)
                {
                    case Line.LineState.None:
                        break;
                    case Line.LineState.Vertical:
                        if (L1.P1 != prevPoint && L1.P1.X != X)
                            L1.P1.MoveLocation(dx, 0, originPoint, this);
                        break;
                    case Line.LineState.Horizontal:
                        if (L1.P1 != prevPoint && L1.P1.Y != Y)
                            L1.P1.MoveLocation(0, dy, originPoint, this);
                        break;
                    case Line.LineState.FixedLength:
                        if (L1.P1 == prevPoint)
                            break;

                        double currentDistance = Math.Sqrt((double)((this.X - L1.P1.X) * (this.X - L1.P1.X)) + (double)((this.Y - L1.P1.Y) * (this.Y - L1.P1.Y)));
                        double wantedDistance = Math.Sqrt((double)L1.LengthSquared);
                        double multiplier = wantedDistance / currentDistance;

                        int mx = (int)Math.Round((double)(L1.P1.X - this.X) * (multiplier - 1));
                        int my = (int)Math.Round((double)(L1.P1.Y - this.Y) * (multiplier - 1));

                        L1.P1.MoveLocation(mx, my, originPoint, this);

                        break;
                }
            }

            if (L2 != null)
            {
                switch (L2.State)
                {
                    case Line.LineState.None:
                        break;
                    case Line.LineState.Vertical:
                        if (L2.P2 != prevPoint && L2.P2.X != X)
                            L2.P2.MoveLocation(dx, 0, originPoint, this);
                        break;
                    case Line.LineState.Horizontal:
                        if (L2.P2 != prevPoint && L2.P2.Y != Y)
                            L2.P2.MoveLocation(0, dy, originPoint, this);
                        break;
                    case Line.LineState.FixedLength:
                        if (L2.P2 == prevPoint)
                            break;

                        double currentDistance = Math.Sqrt((double)((this.X - L2.P2.X) * (this.X - L2.P2.X)) + (double)((this.Y - L2.P2.Y) * (this.Y - L2.P2.Y)));
                        double wantedDistance = Math.Sqrt((double)L2.LengthSquared);
                        double multiplier = wantedDistance / currentDistance;

                        int mx = (int)Math.Round((double)(L2.P2.X - this.X) * (multiplier - 1));
                        int my = (int)Math.Round((double)(L2.P2.Y - this.Y) * (multiplier - 1));

                        L2.P2.MoveLocation(mx, my, originPoint, this);
                        break;
                }
            }
        }

        public void MoveLocationIndependent(int dx, int dy)
        {
            this.X += dx;
            this.Y += dy;
        }
    }

    internal class Line
    {
        public Line(Point p1, Point p2)
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public int LengthSquared { get; private set; } = 0;

        public enum LineState
        {
            None,
            Vertical,
            Horizontal,
            FixedLength,
            Bezier
        };

        public LineState State { get; private set; } = LineState.None;

        public int Distance(int x, int y)
        {
            int distX = int.MaxValue, distY = int.MaxValue;

            if (x >= Math.Min(P1.X, P2.X) && x <= Math.Max(P1.X, P2.X))
            {
                if (P1.X == P2.X)
                    return 0;

                int lineY = (int)Math.Round((double)P1.Y + ((double)(P2.Y - P1.Y) * (double)(x - P1.X) / (double)(P2.X - P1.X)));

                distY = Math.Abs(lineY - y);
            }

            if (y >= Math.Min(P1.Y, P2.Y) && y <= Math.Max(P1.Y, P2.Y))
            {
                if (P1.Y == P2.Y)
                    return 0;

                int lineX = (int)Math.Round((double)P1.X + ((double)(P2.X - P1.X) * (double)(y - P1.Y) / (double)(P2.Y - P1.Y)));

                distX = Math.Abs(lineX - x);
            }

            return Math.Min(distX, distY);
        }

        public bool InBounds(int x, int y)
        {
            return Distance(x, y) <= Polygon.Eps;
        }

        public bool IsDefault()
        {
            return this.State == LineState.None;
        }

        public void ChangeState(LineState newState)
        {
            this.State = newState;

            switch (newState)
            {
                case LineState.Vertical:
                    int avgX = (P1.X + P2.X) / 2;
                    P1.X = P2.X = avgX;
                    break;
                case LineState.Horizontal:
                    int avgY = (P1.Y + P2.Y) / 2;
                    P1.Y = P2.Y = avgY;
                    break;
                case LineState.FixedLength:
                    LengthSquared = (P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y);
                    break;
            }
        }
    }
    internal class Polygon
    {
        public Polygon() { }

        public static readonly int Eps = 5;
        public List<Point> Points = new List<Point>();
        public List<Line> Lines = new List<Line>();

        public int N { get { return Points.Count; } }

        public bool InBounds(int x, int y)
        {
            int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;

            foreach (var point in Points)
            {
                minX = Math.Min(minX, point.X - Eps);
                minY = Math.Min(minY, point.Y - Eps);
                maxX = Math.Max(maxX, point.X + Eps);
                maxY = Math.Max(maxY, point.Y + Eps);
            }

            return minX <= x && minY <= y && maxX >= x && maxY >= y;
        }

        public void DeletePoint(int v)
        {
            Lines[(v - 1 + N) % N].P2 = Points[(v + 1) % N];
            Points[(v + 1) % N].L1 = Lines[(v - 1 + N) % N];

            Lines[(v - 1 + N) % N].ChangeState(Line.LineState.None);

            Points.RemoveAt(v);
            Lines.RemoveAt(v);
        }

        public void DeletePoint(Point p)
        {
            DeletePoint(Points.FindIndex(a => a == p));
        }

        public void AddPoint(int l, Point suggestedPoint)
        {
            Line newLine = new Line(suggestedPoint, Lines[l].P2);

            suggestedPoint.L1 = Lines[l];
            suggestedPoint.L2 = newLine;

            Lines[l].P2 = suggestedPoint;
            Points[(l + 1) % N].L1 = newLine;

            Lines[l].ChangeState(Line.LineState.None);
            Points.Insert(l + 1, suggestedPoint);
            Lines.Insert(l + 1, newLine);
        }

        public void AddPoint(Line l, Point suggestedPoint)
        {
            AddPoint(Lines.FindIndex(a => a == l), suggestedPoint);
        }
    }
}
