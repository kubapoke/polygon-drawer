namespace PolygonDrawer
{
    internal class Line
    {
        public Line(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public double Length
        {
            get
            {
                int dx = P1.X - P2.X, dy = P1.Y - P2.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }
        }
        public double WantedLength { get; set; } = 0;
        public BezierStructure? BezierStructure { get; private set; }

        public enum LineState
        {
            None,
            Vertical,
            Horizontal,
            ForcedLength,
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

                int lineY = (int)Math.Round(P1.Y + (P2.Y - P1.Y) * (double)(x - P1.X) / (P2.X - P1.X));

                distY = Math.Abs(lineY - y);
            }

            if (y >= Math.Min(P1.Y, P2.Y) && y <= Math.Max(P1.Y, P2.Y))
            {
                if (P1.Y == P2.Y)
                    return 0;

                int lineX = (int)Math.Round(P1.X + (P2.X - P1.X) * (double)(y - P1.Y) / (P2.Y - P1.Y));

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
            return State == LineState.None;
        }

        public void ChangeState(LineState newState)
        {
            bool fromBezier = State == LineState.Bezier && newState != LineState.Bezier;

            State = newState;

            if (fromBezier)
            {
                P1.L2 = P2.L1 = this;
                if (!P1.ControlsContinuity) P1.ChangeState(Point.PointState.None);
                if (!P2.ControlsContinuity) P2.ChangeState(Point.PointState.None);
            }

            bool notBezierLine = P1.State != Point.PointState.Bezier && P2.State != Point.PointState.Bezier;

            if (notBezierLine)
            {
                switch (newState)
                {
                    case LineState.None:
                        if (P1.L1 != null && P1.L1.P1.State == Point.PointState.Bezier)
                            P1.L1.ChangeState(LineState.None);
                        if (P2.L2 != null && P2.L2.P2.State == Point.PointState.Bezier)
                            P2.L2.ChangeState(LineState.None);
                        break;
                    case LineState.Vertical:
                        int avgX = (P1.X + P2.X) / 2;
                        P1.MoveLocation(avgX - P1.X, 0, P2);
                        P2.MoveLocation(avgX - P2.X, 0, P1);

                        if (P1.PassesOrientationState && P1.L1 != null && P1.L1.P1.State == Point.PointState.Bezier)
                            P1.L1.ChangeState(LineState.Vertical);
                        if (P2.PassesOrientationState && P2.L2 != null && P2.L2.P2.State == Point.PointState.Bezier)
                            P2.L2.ChangeState(LineState.Vertical);
                        break;
                    case LineState.Horizontal:
                        int avgY = (P1.Y + P2.Y) / 2;
                        P1.MoveLocation(0, avgY - P1.Y, P2);
                        P2.MoveLocation(0, avgY - P2.Y, P1);

                        if (P1.PassesOrientationState && P1.L1 != null && P1.L1.P1.State == Point.PointState.Bezier)
                            P1.L1.ChangeState(LineState.Horizontal);
                        if (P2.PassesOrientationState && P2.L2 != null && P2.L2.P2.State == Point.PointState.Bezier)
                            P2.L2.ChangeState(LineState.Horizontal);
                        break;
                    case LineState.ForcedLength:
                        WantedLength = Length;

                        if (P1.PassesLengthState && P1.L1 != null && P1.L1.P1.State == Point.PointState.Bezier)
                        {
                            P1.L1.ChangeState(LineState.ForcedLength);
                            P1.L1.WantedLength = Length / 3;
                        }
                        if (P2.PassesLengthState && P2.L2 != null && P2.L2.P2.State == Point.PointState.Bezier)
                        {
                            P2.L2.ChangeState(LineState.ForcedLength);
                            P2.L2.WantedLength = Length / 3;
                        }
                        if (!P1.PassesLengthState && P1.PassesOrientationState && P1.L1 != null && P1.L1.P1.State == Point.PointState.Bezier && (P1.L1.State == LineState.Horizontal || P1.L1.State == LineState.Vertical))
                        {
                            P1.L1.ChangeState(LineState.None);
                        }
                        if (!P2.PassesLengthState && P2.PassesOrientationState && P2.L2 != null && P2.L2.P2.State == Point.PointState.Bezier && (P2.L2.State == LineState.Horizontal || P2.L2.State == LineState.Vertical))
                        {
                            P2.L2.ChangeState(LineState.None);
                        }


                        break;
                    case LineState.Bezier:
                        BezierStructure = new BezierStructure(this);
                        if (P1.State == Point.PointState.None)
                            P1.ChangeState(Point.PointState.C1Continuous);
                        if (P2.State == Point.PointState.None)
                            P2.ChangeState(Point.PointState.C1Continuous);
                        if (P1.L1 != null && P1.L1.State != LineState.None && P1.L1.State != LineState.Bezier)
                            P1.L1.ChangeState(P1.L1.State);
                        if (P2.L2 != null && P2.L2.State != LineState.None && P2.L2.State != LineState.Bezier)
                            P2.L2.ChangeState(P2.L2.State);
                        break;
                }

                P1.MoveLocation(0, 0);
                P2.MoveLocation(0, 0);
            }
        }

        public void SetWantedLength(double length)
        {
            WantedLength = length;

            int midX = (int)Math.Round((P1.X + P2.X) / 2.0);
            int midY = (int)Math.Round((P1.Y + P2.Y) / 2.0);

            double dx = P2.X - P1.X;
            double dy = P2.Y - P1.Y;
            double currentLength = Length;

            double unitX = dx / currentLength;
            double unitY = dy / currentLength;

            double halfLength = length / 2.0;
            int offsetX = (int)Math.Round(unitX * halfLength);
            int offsetY = (int)Math.Round(unitY * halfLength);

            P1.MoveLocation(-P1.X + midX - offsetX, -P1.Y + midY - offsetY, P2);
            P2.MoveLocation(-P2.X + midX + offsetX, -P2.Y + midY + offsetY, P1);

            if (P1.PassesLengthState && P1.L1 != null && P1.L1.P1.State == Point.PointState.Bezier)
            {
                P1.L1.WantedLength = Length / 3;
                P1.MoveLocation(0, 0);
            }
            if (P2.PassesLengthState && P2.L2 != null && P2.L2.P2.State == Point.PointState.Bezier)
            {
                P2.L2.WantedLength = Length / 3;
                P2.MoveLocation(0, 0);
            }
        }
    }
}
