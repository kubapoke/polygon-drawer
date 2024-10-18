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

        public static (int, int) GetNewPositionAfterRotation(Point P0, Point P1, Point P2)
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

            return (newX - P2.X, newY - P2.Y);
        }

        public static (int, int) GetNewPositionAfterRotationWithLength(Point P0, Point P1, Point P2)
        {
            double dx01 = P1.X - P0.X;
            double dy01 = P1.Y - P0.Y;

            double lengthP0P1 = Math.Sqrt(dx01 * dx01 + dy01 * dy01);

            double unitX = dx01 / lengthP0P1;
            double unitY = dy01 / lengthP0P1;

            int newX = (int)Math.Round(P1.X + unitX * lengthP0P1);
            int newY = (int)Math.Round(P1.Y + unitY * lengthP0P1);

            return (newX - P2.X, newY - P2.Y);
        }

        public static (int, int) GetNewPositionAfterRotationWithFixedLength(Point P0, Point P1, Point P2, double length)
        {
            double dx01 = P1.X - P0.X;
            double dy01 = P1.Y - P0.Y;

            double lengthP0P1 = Math.Sqrt(dx01 * dx01 + dy01 * dy01);

            double unitX = dx01 / lengthP0P1;
            double unitY = dy01 / lengthP0P1;

            int newX = (int)Math.Round(P1.X + unitX * length);
            int newY = (int)Math.Round(P1.Y + unitY * length);

            return (newX - P2.X, newY - P2.Y);
        }

        public static (int, int) GetNewPositionAfterRotationHorizontalKeepLength(Point P0, Point P1, Point P2)
        {
            double lengthP1P2 = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));

            int newX = (int)Math.Round(P1.X + Math.Sign(P1.X - P0.X) * Math.Abs(lengthP1P2));
            int newY = P1.Y;

            return (newX - P2.X, newY - P2.Y);
        }

        public static (int, int) GetNewPositionAfterRotationHorizontalP0P1(Point P0, Point P1, Point P2)
        {
            double lengthP0P1 = Math.Sqrt(Math.Pow(P1.X - P0.X, 2) + Math.Pow(P1.Y - P0.Y, 2));

            int newX = (int)Math.Round(P1.X + Math.Sign(P1.X - P0.X) * Math.Abs(lengthP0P1));
            int newY = P1.Y;

            return (newX - P2.X, newY - P2.Y);
        }

        public static (int, int) GetNewPositionAfterRotationVerticalKeepLength(Point P0, Point P1, Point P2)
        {
            double lengthP1P2 = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));

            int newX = P1.X;
            int newY = (int)Math.Round(P1.Y + Math.Sign(P1.Y - P0.Y) * Math.Abs(lengthP1P2));

            return (newX - P2.X, newY - P2.Y);
        }

        public static (int, int) GetNewPositionAfterRotationVerticalP0P1(Point P0, Point P1, Point P2)
        {
            double lengthP0P1 = Math.Sqrt(Math.Pow(P1.X - P0.X, 2) + Math.Pow(P1.Y - P0.Y, 2));

            int newX = P1.X;
            int newY = (int)Math.Round(P1.Y + Math.Sign(P1.Y - P0.Y) * Math.Abs(lengthP0P1));

            return (newX - P2.X, newY - P2.Y);
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

            if (L1 != null && L1.P1 != prevPoint)
            {
                (int x, int y) howToMove = (0, 0);
                if (PassesOrientationState && State == PointState.G1Continuous && L2 != null && !(prevPoint == null && L1.P1.State != PointState.Bezier))
                {
                    switch (L1.State)
                    {
                        case Line.LineState.ForcedLength:
                            howToMove = GetNewPositionAfterRotationWithFixedLength(L2.P2, this, L1.P1, L1.WantedLength);
                            break;
                        case Line.LineState.Horizontal:
                            howToMove = GetNewPositionAfterRotationHorizontalKeepLength(L2.P2, this, L1.P1);
                            break;
                        case Line.LineState.Vertical:
                            howToMove = GetNewPositionAfterRotationVerticalKeepLength(L2.P2, this, L1.P1);
                            break;
                        default:
                            howToMove = GetNewPositionAfterRotation(L2.P2, this, L1.P1);
                            break;
                    }
                    L1.P1.MoveLocation(howToMove.x, howToMove.y, originPoint, this);
                }
                else if (PassesOrientationState && State == PointState.C1Continuous && L2 != null && !(prevPoint == null && L1.P1.State != PointState.Bezier))
                {
                    switch (L1.State)
                    {
                        case Line.LineState.ForcedLength:
                            howToMove = GetNewPositionAfterRotationWithFixedLength(L2.P2, this, L1.P1, L1.WantedLength);
                            break;
                        case Line.LineState.Horizontal:
                            howToMove = GetNewPositionAfterRotationHorizontalP0P1(L2.P2, this, L1.P1);
                            break;
                        case Line.LineState.Vertical:
                            howToMove = GetNewPositionAfterRotationVerticalP0P1(L2.P2, this, L1.P1);
                            break;
                        default:
                            howToMove = GetNewPositionAfterRotationWithLength(L2.P2, this, L1.P1);
                            break;
                    }
                    L1.P1.MoveLocation(howToMove.x, howToMove.y, originPoint, this);
                }
                else
                {
                    switch (L1.State)
                    {
                        case Line.LineState.None:
                            if (L1.P1.ControlsContinuity || this.ControlsContinuity)
                                L1.P1.MoveLocation(0, 0, originPoint, this);
                            break;
                        case Line.LineState.Vertical:
                            L1.P1.MoveLocation(X - L1.P1.X, 0, originPoint, this);
                            break;
                        case Line.LineState.Horizontal:

                            L1.P1.MoveLocation(0, Y - L1.P1.Y, originPoint, this);
                            break;
                        case Line.LineState.ForcedLength:
                            double currentDistance = Math.Sqrt((X - L1.P1.X) * (X - L1.P1.X) + (double)((Y - L1.P1.Y) * (Y - L1.P1.Y)));
                            double wantedDistance = L1.WantedLength;
                            double multiplier = wantedDistance / currentDistance - 1;

                            int mx = (int)Math.Round((L1.P1.X - X) * multiplier);
                            int my = (int)Math.Round((L1.P1.Y - Y) * multiplier);

                            L1.P1.MoveLocation(mx, my, originPoint, this);

                            break;
                    }
                }
            }

            if (L2 != null && L2.P2 != prevPoint)
            {
                (int x, int y) howToMove = (0, 0);
                if (PassesOrientationState && State == PointState.G1Continuous && L1 != null && !(prevPoint == null && L2.P2.State != PointState.Bezier))
                {
                    switch (L2.State)
                    {
                        case Line.LineState.ForcedLength:
                            howToMove = GetNewPositionAfterRotationWithFixedLength(L1.P1, this, L2.P2, L2.WantedLength);
                            break;
                        case Line.LineState.Horizontal:
                            howToMove = GetNewPositionAfterRotationHorizontalKeepLength(L1.P1, this, L2.P2);
                            break;
                        case Line.LineState.Vertical:
                            howToMove = GetNewPositionAfterRotationVerticalKeepLength(L1.P1, this, L2.P2);
                            break;
                        default:
                            howToMove = GetNewPositionAfterRotation(L1.P1, this, L2.P2);
                            break;
                    }
                    L2.P2.MoveLocation(howToMove.x, howToMove.y, originPoint, this);
                }
                else if (PassesOrientationState && State == PointState.C1Continuous && L1 != null && !(prevPoint == null && L2.P2.State != PointState.Bezier))
                {
                    switch (L2.State)
                    {
                        case Line.LineState.ForcedLength:
                            howToMove = GetNewPositionAfterRotationWithFixedLength(L1.P1, this, L2.P2, L2.WantedLength);
                            break;
                        case Line.LineState.Horizontal:
                            howToMove = GetNewPositionAfterRotationHorizontalP0P1(L1.P1, this, L2.P2);
                            break;
                        case Line.LineState.Vertical:
                            howToMove = GetNewPositionAfterRotationVerticalP0P1(L1.P1, this, L2.P2);
                            break;
                        default:
                            howToMove = GetNewPositionAfterRotationWithLength(L1.P1, this, L2.P2);
                            break;
                    }
                    L2.P2.MoveLocation(howToMove.x, howToMove.y, originPoint, this);
                }
                else
                {
                    switch (L2.State)
                    {
                        case Line.LineState.None:
                            if (L2.P2.ControlsContinuity || this.ControlsContinuity)
                                L2.P2.MoveLocation(0, 0, originPoint, this);
                            break;
                        case Line.LineState.Vertical:
                            L2.P2.MoveLocation(X - L2.P2.X, 0, originPoint, this);
                            break;
                        case Line.LineState.Horizontal:
                            L2.P2.MoveLocation(0, Y - L2.P2.Y, originPoint, this);
                            break;
                        case Line.LineState.ForcedLength:
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
        }

        public void MoveLocationIndependent(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public void ChangeState(PointState state)
        {
            State = state;

            switch (state)
            {
                case PointState.G0Continuous:
                    if (L1 != null && L1.P1.State == PointState.Bezier)
                    {
                        L1.ChangeState(Line.LineState.None);
                    }
                    if (L2 != null && L2.P2.State == PointState.Bezier)
                    {
                        L2.ChangeState(Line.LineState.None);
                    }
                    break;
                case PointState.G1Continuous:
                    if (L1 != null && L1.P1.State == PointState.Bezier && L1.State == Line.LineState.ForcedLength)
                    {
                        L1.ChangeState(Line.LineState.None);
                    }
                    if (L2 != null && L2.P2.State == PointState.Bezier && L2.State == Line.LineState.ForcedLength)
                    {
                        L2.ChangeState(Line.LineState.None);
                    }
                    if (L1 != null && L2 != null && L1.P1.State == PointState.Bezier && (L2.State == Line.LineState.Horizontal || L2.State == Line.LineState.Vertical))
                    {
                        L1.ChangeState(L2.State);
                    }
                    if (L2 != null && L1 != null && L2.P2.State == PointState.Bezier && (L1.State == Line.LineState.Horizontal || L1.State == Line.LineState.Vertical))
                    {
                        L2.ChangeState(L1.State);
                    }
                    break;
                case PointState.C1Continuous:
                    if (L1 != null && L2 != null && L1.P1.State == PointState.Bezier && L2.State != Line.LineState.Bezier && L2.State != Line.LineState.None)
                    {
                        L1.ChangeState(L2.State);
                        L1.WantedLength = L2.WantedLength;
                    }
                    if (L2 != null && L1 != null && L2.P2.State == PointState.Bezier && L1.State != Line.LineState.Bezier && L1.State != Line.LineState.None)
                    {
                        L2.ChangeState(L1.State);
                        L2.WantedLength = L1.WantedLength;
                    }
                    break;
            }

            MoveLocation(0, 0);
        }
    }
}
