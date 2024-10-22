namespace PolygonDrawer
{
    internal class Line
    {
        public Line(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
            BezierStructure = new BezierStructure(null); // placeholder bezier structure
        }

        public Point[] P = new Point[2];

        public Point P1
        {
            get { return P[0]; }
            set { P[0] = value; }
        }
        public Point P2
        {
            get { return P[1]; }
            set { P[1] = value; }
        }
        public Line L1
        {
            get { return P1.L1; }
            set { P1.L1 = value; }
        }
        public Line L2
        {
            get { return P2.L2; }
            set { P2.L2 = value; }
        }

        public double Length
        {
            get
            {
                int dx = P1.X - P2.X, dy = P1.Y - P2.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }
        }
        public double WantedLength { get; set; } = 0;
        public BezierStructure BezierStructure { get; private set; }

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
                for (int i = 0; i < 2; i++)
                {
                    switch (newState)
                    {
                        case LineState.None:
                            if (P[i].L[i].P[i].State == Point.PointState.Bezier)
                                P[i].L[i].ChangeState(LineState.None);
                            break;
                        case LineState.Vertical:
                            int avgX = (P[0].X + P[1].X) / 2;
                            P[0].MoveLocation(avgX - P[0].X, 0, P[1]);
                            P[1].MoveLocation(avgX - P[1].X, 0, P[0]);

                            if (P[i].PassesOrientationState && P[i].L[i].P[i].State == Point.PointState.Bezier)
                                P[i].L[i].ChangeState(LineState.Vertical);
                            break;
                        case LineState.Horizontal:
                            int avgY = (P[0].Y + P[1].Y) / 2;
                            P[0].MoveLocation(0, avgY - P[0].Y, P[1]);
                            P[1].MoveLocation(0, avgY - P[1].Y, P[0]);

                            if (P[i].PassesOrientationState && P[i].L[i].P[i].State == Point.PointState.Bezier)
                                P[i].L[i].ChangeState(LineState.Horizontal);
                            break;
                        case LineState.ForcedLength:
                            WantedLength = Length;

                            if (P[i].PassesLengthState && P[i].L[i].P[i].State == Point.PointState.Bezier)
                            {
                                P[i].L[i].ChangeState(LineState.ForcedLength);
                                P[i].L[i].WantedLength = Length / 3;
                            }
                            if (!P[i].PassesLengthState && P[i].PassesOrientationState && P[i].L[i].P[i].State == Point.PointState.Bezier &&
                                (P[i].L[i].State == LineState.Horizontal || P[i].L[i].State == LineState.Vertical))
                            {
                                P[i].L[i].ChangeState(LineState.None);
                            }
                            break;
                        case LineState.Bezier:
                            BezierStructure = new BezierStructure(this);
                            if (P[i].State == Point.PointState.None)
                                P[i].ChangeState(Point.PointState.C1Continuous);

                            if (P[(i + 1) % 2].L[(i + 1) % 2].P[(i + 1) % 2].State == Point.PointState.Bezier)
                                P[(i + 1) % 2].L[(i + 1) % 2].ChangeState(LineState.None);

                            if (P[i].L[i].State != LineState.None && P[i].L[i].State != LineState.Bezier)
                                P[i].L[i].ChangeState(P[i].L[i].State);
                            break;
                    }
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

            if (P1.PassesLengthState && P1.L1.P1.State == Point.PointState.Bezier)
            {
                P1.L1.WantedLength = Length / 3;
                P1.MoveLocation(0, 0);
            }
            if (P2.PassesLengthState && P2.L2.P2.State == Point.PointState.Bezier)
            {
                P2.L2.WantedLength = Length / 3;
                P2.MoveLocation(0, 0);
            }
        }
    }
}
