namespace PolygonDrawer
{
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Line? L1 { get; set; }
        public Line? L2 { get; set; }
        public BezierStructure? BezierStructure { get; set; }
        public bool ControlsContinuity
        {
            get
            {
                if (State == PointState.Bezier)
                    return false;

                if (L1 != null)
                    if (L1.P1.State == PointState.Bezier)
                        return true;

                if (L2 != null)
                    if (L2.P2.State == PointState.Bezier)
                        return true;

                return false;
            }
        }

        public bool PassesOrientationState
        {
            get
            {
                return ControlsContinuity && (State == PointState.G1Continuous || State == PointState.C1Continuous);
            }
        }

        public bool PassesLengthState
        {
            get
            {
                return ControlsContinuity && State == PointState.C1Continuous;
            }
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public enum PointState
        {
            None,
            G0Continuous,
            G1Continuous,
            C1Continuous,
            Bezier
        }

        public PointState State { get; private set; } = PointState.None;

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

        public static void AdjustRotation(Point P0, Point P1, Point P2)
        {
            double dx01 = P1.X - P0.X;
            double dy01 = P1.Y - P0.Y;

            double lengthP1P2 = Math.Sqrt((P2.X - P1.X) * (P2.X - P1.X) +
                                          (P2.Y - P1.Y) * (P2.Y - P1.Y));

            double lengthP0P1 = Math.Sqrt(dx01 * dx01 + dy01 * dy01);
            double unitX = dx01 / lengthP0P1;
            double unitY = dy01 / lengthP0P1;

            int newX = (int)Math.Round(P1.X + unitX * lengthP1P2);
            int newY = (int)Math.Round(P1.Y + unitY * lengthP1P2);

            P2.X = newX;
            P2.Y = newY;
        }

        public static void AdjustRotationWithLength(Point P0, Point P1, Point P2)
        {
            double dx01 = P1.X - P0.X;
            double dy01 = P1.Y - P0.Y;

            double lengthP0P1 = Math.Sqrt(dx01 * dx01 + dy01 * dy01);

            double unitX = dx01 / lengthP0P1;
            double unitY = dy01 / lengthP0P1;

            int newX = (int)Math.Round(P1.X + unitX * lengthP0P1);
            int newY = (int)Math.Round(P1.Y + unitY * lengthP0P1);

            P2.X = newX;
            P2.Y = newY;
        }


        public void MoveLocation(int dx, int dy, Point? originPoint = null, Point? prevPoint = null)
        {
            originPoint = originPoint ?? this;

            if (this == originPoint && prevPoint != null)
                return;

            if (this.ControlsContinuity && (dx != 0 || dy != 0))
            {
                if (L1 != null && L1.P1.State == PointState.Bezier && L1.P1 != prevPoint && L1.P1.BezierStructure != null)
                {
                    L1.P1.BezierStructure.AdjustCurvePoints(this, dx, dy, originPoint);
                }
                if (L2 != null && L2.P2.State == PointState.Bezier && L2.P2 != prevPoint && L2.P2.BezierStructure != null)
                {
                    L2.P2.BezierStructure.AdjustCurvePoints(this, dx, dy, originPoint);
                }
            }

            X += dx;
            Y += dy;

            if (prevPoint != null && prevPoint.ControlsContinuity)
            {
                if (prevPoint.State == PointState.G1Continuous)
                {
                    if (L1 != null && L1.P1 == prevPoint && prevPoint.L1 != null)
                    {
                        if (this.State == PointState.Bezier && prevPoint.L1.P1 != originPoint)
                            AdjustRotation(this, prevPoint, prevPoint.L1.P1);
                        else
                            AdjustRotation(prevPoint.L1.P1, prevPoint, this);
                    }
                    else if (L2 != null && L2.P2 == prevPoint && prevPoint.L2 != null)
                    {
                        if (this.State == PointState.Bezier && prevPoint.L2.P2 != originPoint)
                            AdjustRotation(this, prevPoint, prevPoint.L2.P2);
                        else
                            AdjustRotation(prevPoint.L2.P2, prevPoint, this);
                    }
                }
                else if (prevPoint.State == PointState.C1Continuous)
                {
                    if (L1 != null && L1.P1 == prevPoint && prevPoint.L1 != null)
                    {
                        if (this.State == PointState.Bezier && prevPoint.L1.P1 != originPoint)
                            AdjustRotationWithLength(this, prevPoint, prevPoint.L1.P1);
                        else
                            AdjustRotationWithLength(prevPoint.L1.P1, prevPoint, this);
                    }
                    else if (L2 != null && L2.P2 == prevPoint && prevPoint.L2 != null)
                    {
                        if (this.State == PointState.Bezier && prevPoint.L2.P2 != originPoint)
                            AdjustRotationWithLength(this, prevPoint, prevPoint.L2.P2);
                        else
                            AdjustRotationWithLength(prevPoint.L2.P2, prevPoint, this);
                    }
                }
            }

            if (L1 != null)
            {
                switch (L1.State)
                {
                    case Line.LineState.None:
                        if (L1.P1 != prevPoint && (L1.P1.ControlsContinuity || this.ControlsContinuity))
                            L1.P1.MoveLocation(0, 0, originPoint, this);
                        break;
                    case Line.LineState.Vertical:
                        if (L1.P1 != prevPoint)
                            L1.P1.MoveLocation(X - L1.P1.X, 0, originPoint, this);
                        break;
                    case Line.LineState.Horizontal:
                        if (L1.P1 != prevPoint)
                            L1.P1.MoveLocation(0, Y - L1.P1.Y, originPoint, this);
                        break;
                    case Line.LineState.ForcedLength:
                        if (L1.P1 == prevPoint)
                            break;

                        double currentDistance = Math.Sqrt((X - L1.P1.X) * (X - L1.P1.X) + (double)((Y - L1.P1.Y) * (Y - L1.P1.Y)));
                        double wantedDistance = L1.WantedLength;
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
                        if (L2.P2 != prevPoint && (L2.P2.ControlsContinuity || this.ControlsContinuity))
                            L2.P2.MoveLocation(0, 0, originPoint, this);
                        break;
                    case Line.LineState.Vertical:
                        if (L2.P2 != prevPoint)
                            L2.P2.MoveLocation(X - L2.P2.X, 0, originPoint, this);
                        break;
                    case Line.LineState.Horizontal:
                        if (L2.P2 != prevPoint)
                            L2.P2.MoveLocation(0, Y - L2.P2.Y, originPoint, this);
                        break;
                    case Line.LineState.ForcedLength:
                        if (L2.P2 == prevPoint)
                            break;

                        double currentDistance = Math.Sqrt((X - L2.P2.X) * (X - L2.P2.X) + (double)((Y - L2.P2.Y) * (Y - L2.P2.Y)));
                        double wantedDistance = L2.WantedLength;
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

        public void ChangeState(PointState state)
        {
            State = state;
            MoveLocation(0, 0);
        }
    }
}
