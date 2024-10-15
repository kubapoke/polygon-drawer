﻿namespace PolygonDrawer
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
        public double Length { get; private set; } = 0;

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
            State = newState;

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
                    Length = Math.Sqrt((P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y));
                    break;
            }
        }
    }
}
