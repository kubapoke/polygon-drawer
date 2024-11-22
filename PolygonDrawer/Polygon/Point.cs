using System.Numerics;

namespace PolygonDrawer
{
    internal class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
            L1 = L2 = new Line(this, this); //placeholder line
            BezierStructure = new BezierStructure(null);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public Line[] L = new Line[2];
        public Line L1
        {
            get { return L[0]; }
            set { L[0] = value; }
        }
        public Line L2
        {
            get { return L[1]; }
            set { L[1] = value; }
        }
        public Point P1
        {
            get { return L1.P1; }
            set { L1.P1 = value; }
        }
        public Point P2
        {
            get { return L2.P2; }
            set { L2.P2 = value; }
        }
        public BezierStructure BezierStructure { get; set; }

        public bool ControlsContinuity
        {
            get
            {
                if (State == PointState.Bezier)
                    return false;

                if (L1.P1.State == PointState.Bezier)
                    return true;

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

        public enum PointState
        {
            None,
            G0Continuous,
            G1Continuous,
            C1Continuous,
            Bezier
        }

        public PointState State { get; private set; } = PointState.None;

        public double Distance(double x, double y)
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

        public static (double, double) GetPositionAfterRotation(Point P0, Point P1, Point P2)
        {
            if ((P0.X == P1.X && P0.Y == P1.Y) || (P0.X == P2.X && P0.Y == P2.Y) || (P1.X == P2.X && P1.Y == P2.Y))
                return (0, 0);

            double dx01 = P1.X - P0.X;
            double dy01 = P1.Y - P0.Y;

            double lengthP1P2 = Math.Sqrt((P2.X - P1.X) * (P2.X - P1.X) +
                                          (P2.Y - P1.Y) * (P2.Y - P1.Y));

            double lengthP0P1 = Math.Sqrt(dx01 * dx01 + dy01 * dy01);
            double unitX = dx01 / lengthP0P1;
            double unitY = dy01 / lengthP0P1;

            double newX = P1.X + unitX * lengthP1P2;
            double newY = P1.Y + unitY * lengthP1P2;

            return (newX - P2.X, newY - P2.Y);
        }

        public static (double, double) GetPositionAfterRotationCopyLength(Point P0, Point P1, Point P2)
        {
            if ((P0.X == P1.X && P0.Y == P1.Y) || (P0.X == P2.X && P0.Y == P2.Y) || (P1.X == P2.X && P1.Y == P2.Y))
                return (0, 0);

            double dx01 = P1.X - P0.X;
            double dy01 = P1.Y - P0.Y;

            double lengthP0P1 = Math.Sqrt(dx01 * dx01 + dy01 * dy01);

            double unitX = dx01 / lengthP0P1;
            double unitY = dy01 / lengthP0P1;

            if (P0.State == PointState.Bezier && P2.State != PointState.Bezier)
                lengthP0P1 *= 3.0;
            else if (P0.State != PointState.Bezier && P2.State == PointState.Bezier)
                lengthP0P1 /= 3.0;

            double newX = P1.X + unitX * lengthP0P1;
            double newY = P1.Y + unitY * lengthP0P1;

            return (newX - P2.X, newY - P2.Y);
        }

        public static (double, double) GetPositionAfterRotationFixedLength(Point P0, Point P1, Point P2, double length)
        {
            double dx01 = P1.X - P0.X;
            double dy01 = P1.Y - P0.Y;

            double lengthP0P1 = Math.Sqrt(dx01 * dx01 + dy01 * dy01);

            double unitX = dx01 / lengthP0P1;
            double unitY = dy01 / lengthP0P1;

            double newX = P1.X + unitX * length;
            double newY = P1.Y + unitY * length;

            return (newX - P2.X, newY - P2.Y);
        }

        public static (double, double) GetPositionAfterRotationHorizontal(Point P0, Point P1, Point P2)
        {
            double lengthP1P2 = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));

            if (P0.X == P1.X && P0.Y == P1.Y)
                return (0, 0);

            double newX = P1.X + Math.Sign(P1.X - P0.X) * Math.Abs(lengthP1P2);
            double newY = P1.Y;

            return (newX - P2.X, newY - P2.Y);
        }


        public static (double, double) GetPositionAfterRotationCopyLengthHorizontal(Point P0, Point P1, Point P2)
        {
            double lengthP0P1 = Math.Sqrt(Math.Pow(P1.X - P0.X, 2) + Math.Pow(P1.Y - P0.Y, 2));

            if (P0.State == PointState.Bezier && P2.State != PointState.Bezier)
                lengthP0P1 *= 3.0;
            else if (P0.State != PointState.Bezier && P2.State == PointState.Bezier)
                lengthP0P1 /= 3.0;

            double newX = P1.X + Math.Sign(P1.X - P0.X) * Math.Abs(lengthP0P1);
            double newY = P1.Y;

            return (newX - P2.X, newY - P2.Y);
        }

        public static (double, double) GetPositionAfterRotationVertical(Point P0, Point P1, Point P2)
        {
            double lengthP1P2 = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));

            if (P0.X == P1.X && P0.Y == P1.Y)
                return (0, 0);

            double newX = P1.X;
            double newY = P1.Y + Math.Sign(P1.Y - P0.Y) * Math.Abs(lengthP1P2);

            return (newX - P2.X, newY - P2.Y);
        }

        public static (double, double) GetPositionAfterRotationCopyLengthVertical(Point P0, Point P1, Point P2)
        {
            double lengthP0P1 = Math.Sqrt(Math.Pow(P1.X - P0.X, 2) + Math.Pow(P1.Y - P0.Y, 2));

            if (P0.State == PointState.Bezier && P2.State != PointState.Bezier)
                lengthP0P1 *= 3.0;
            else if (P0.State != PointState.Bezier && P2.State == PointState.Bezier)
                lengthP0P1 /= 3.0;

            double newX = P1.X;
            double newY = P1.Y + Math.Sign(P1.Y - P0.Y) * Math.Abs(lengthP0P1);

            return (newX - P2.X, newY - P2.Y);
        }

        public void MoveLocation(double dx, double dy, Point? originPoint = null, Point? prevPoint = null)
        {
            originPoint = originPoint ?? this;

            if (this == originPoint && prevPoint != null)
                return;

            if (this.ControlsContinuity && (dx != 0 || dy != 0))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (L[i].P[i].State == PointState.Bezier && L[i].P[i] != prevPoint)
                    {
                        L[i].P[i].BezierStructure.AdjustCurvePoints(this, dx, dy, originPoint);
                    }
                }
            }

            X += dx;
            Y += dy;

            for (int i = 0; i < 2; i++)
            {
                if (L[i].P[i] != prevPoint)
                {
                    (double x, double y) howToMove = (0, 0);
                    if (PassesOrientationState && State == PointState.G1Continuous && !(i == 0 && prevPoint == null && L[i].P[i].State != PointState.Bezier))
                    {
                        switch (L[i].State)
                        {
                            case Line.LineState.FixedLength:
                                howToMove = GetPositionAfterRotationFixedLength(L[1 - i].P[1 - i], this, L[i].P[i], L[i].WantedLength);
                                break;
                            case Line.LineState.Horizontal:
                                howToMove = GetPositionAfterRotationHorizontal(L[1 - i].P[1 - i], this, L[i].P[i]);
                                break;
                            case Line.LineState.Vertical:
                                howToMove = GetPositionAfterRotationVertical(L[1 - i].P[1 - i], this, L[i].P[i]);
                                break;
                            default:
                                howToMove = GetPositionAfterRotation(L[1 - i].P[1 - i], this, L[i].P[i]);
                                break;
                        }
                        L[i].P[i].MoveLocation(howToMove.x, howToMove.y, originPoint, this);
                    }
                    else if (PassesOrientationState && State == PointState.C1Continuous && !(i == 0 && prevPoint == null && L[i].P[i].State != PointState.Bezier))
                    {
                        switch (L[i].State)
                        {
                            case Line.LineState.FixedLength:
                                howToMove = GetPositionAfterRotationFixedLength(L[1 - i].P[1 - i], this, L[i].P[i], L[i].WantedLength);
                                break;
                            case Line.LineState.Horizontal:
                                howToMove = GetPositionAfterRotationCopyLengthHorizontal(L[1 - i].P[1 - i], this, L[i].P[i]);
                                break;
                            case Line.LineState.Vertical:
                                howToMove = GetPositionAfterRotationCopyLengthVertical(L[1 - i].P[1 - i], this, L[i].P[i]);
                                break;
                            default:
                                howToMove = GetPositionAfterRotationCopyLength(L[1 - i].P[1 - i], this, L[i].P[i]);
                                break;
                        }
                        L[i].P[i].MoveLocation(howToMove.x, howToMove.y, originPoint, this);
                    }
                    else
                    {
                        switch (L[i].State)
                        {
                            case Line.LineState.None:
                                if (L[i].P[i].ControlsContinuity || this.ControlsContinuity)
                                    L[i].P[i].MoveLocation(0, 0, originPoint, this);
                                break;
                            case Line.LineState.Vertical:
                                L[i].P[i].MoveLocation(X - L[i].P[i].X, 0, originPoint, this);
                                break;
                            case Line.LineState.Horizontal:
                                L[i].P[i].MoveLocation(0, Y - L[i].P[i].Y, originPoint, this);
                                break;
                            case Line.LineState.FixedLength:
                                double currentDistance = Math.Sqrt((X - L[i].P[i].X) * (X - L[i].P[i].X) + (double)((Y - L[i].P[i].Y) * (Y - L[i].P[i].Y)));
                                double wantedDistance = L[i].WantedLength;
                                double multiplier = wantedDistance / currentDistance - 1;

                                int mx = (int)Math.Round((L[i].P[i].X - X) * multiplier);
                                int my = (int)Math.Round((L[i].P[i].Y - Y) * multiplier);

                                L[i].P[i].MoveLocation(mx, my, originPoint, this);

                                break;
                        }
                    }
                }
            }
        }

        public void MoveLocationIndependent(double dx, double dy)
        {
            X += dx;
            Y += dy;
        }

        public void ChangeState(PointState state)
        {
            State = state;

            for (int i = 0; i < 2; i++)
            {
                switch (state)
                {
                    case PointState.G0Continuous:
                        if (L[i].P[i].State == PointState.Bezier)
                        {
                            L[i].ChangeState(Line.LineState.None);
                        }
                        break;
                    case PointState.G1Continuous:
                        if (L[i].P[i].State == PointState.Bezier && L[i].State == Line.LineState.FixedLength)
                        {
                            L[i].ChangeState(Line.LineState.None);
                        }
                        if (L[i].P[i].State == PointState.Bezier && (L[1 - i].State == Line.LineState.Horizontal || L[1 - i].State == Line.LineState.Vertical))
                        {
                            L[i].ChangeState(L[1 - i].State);
                        }
                        break;
                    case PointState.C1Continuous:
                        if (L[i].P[i].State == PointState.Bezier && L[1 - i].State != Line.LineState.Bezier && L[1 - i].State != Line.LineState.None)
                        {
                            L[i].ChangeState(L[1 - i].State);
                            L[i].WantedLength = L[1 - i].WantedLength / 3;
                        }
                        break;
                }
            }

            MoveLocation(0, 0);
        }

        public static implicit operator Vector2(Point point)
        {
            return new Vector2((float)point.X, (float)point.Y);
        }
    }
}
