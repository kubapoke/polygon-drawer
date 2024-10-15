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
            return Math.Max(Math.Abs(x - X), Math.Abs(y - Y));
        }

        public bool InBounds(int x, int y)
        {
            return Distance(x, y) <= Polygon.Eps;
        }

        public void SetLocation(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveLocation(int dx, int dy, Point? originPoint = null, Point? prevPoint = null)
        {
            if (this == originPoint && prevPoint != null)
                return;

            X += dx;
            Y += dy;

            if (L1 != null)
            {
                switch (L1.State)
                {
                    case Line.LineState.None:
                        break;
                    case Line.LineState.Vertical:
                        if (L1.P1 != prevPoint && L1.P1.X != X)
                            L1.P1.MoveLocation(X - L1.P1.X, 0, originPoint, this);
                        break;
                    case Line.LineState.Horizontal:
                        if (L1.P1 != prevPoint && L1.P1.Y != Y)
                            L1.P1.MoveLocation(0, Y - L1.P1.Y, originPoint, this);
                        break;
                    case Line.LineState.FixedLength:
                        if (L1.P1 == prevPoint)
                            break;

                        double currentDistance = Math.Sqrt((X - L1.P1.X) * (X - L1.P1.X) + (double)((Y - L1.P1.Y) * (Y - L1.P1.Y)));
                        double wantedDistance = L1.Length;
                        double multiplier = wantedDistance / currentDistance - 1;

                        int mx = (int)Math.Round((L1.P1.X - X) * multiplier);
                        int my = (int)Math.Round((L1.P1.Y - Y) * multiplier);

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
                            L2.P2.MoveLocation(X - L2.P2.X, 0, originPoint, this);
                        break;
                    case Line.LineState.Horizontal:
                        if (L2.P2 != prevPoint && L2.P2.Y != Y)
                            L2.P2.MoveLocation(0, Y - L2.P2.Y, originPoint, this);
                        break;
                    case Line.LineState.FixedLength:
                        if (L2.P2 == prevPoint)
                            break;

                        double currentDistance = Math.Sqrt((X - L2.P2.X) * (X - L2.P2.X) + (double)((Y - L2.P2.Y) * (Y - L2.P2.Y)));
                        double wantedDistance = L2.Length;
                        double multiplier = wantedDistance / currentDistance - 1;

                        int mx = (int)Math.Round((L2.P2.X - X) * multiplier);
                        int my = (int)Math.Round((L2.P2.Y - Y) * multiplier);

                        L2.P2.MoveLocation(mx, my, originPoint, this);
                        break;
                }
            }
        }

        public void MoveLocationIndependent(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }
    }
}
