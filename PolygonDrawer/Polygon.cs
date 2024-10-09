namespace PolygonDrawer
{
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

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
            Lines[l].P2 = suggestedPoint;
            Points.Insert(l + 1, suggestedPoint);
            Lines.Insert(l + 1, newLine);
        }

        public void AddPoint(Line l, Point suggestedPoint)
        {
            AddPoint(Lines.FindIndex(a => a == l), suggestedPoint);
        }
    }
}
