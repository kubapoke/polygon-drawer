namespace PolygonDrawer
{
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

        public void Draw(Graphics g, Action<Graphics, int, int, int, int, Color?> drawLineAction,
            System.Drawing.Point relativeMousePos, bool creatingNewPolygon)
        {
            foreach (var point in Points)
            {
                var rectangle = new Rectangle(point.X - Eps, point.Y - Eps, 2 * Eps, 2 * Eps);
                g.FillEllipse(new SolidBrush(Color.Black), rectangle);
            }

            foreach (var line in Lines)
            {
                drawLineAction(g, line.P1.X, line.P1.Y, line.P2.X, line.P2.Y, null);
            }

            if (creatingNewPolygon && N > 0)
            {
                drawLineAction(g, Points[N - 1].X, Points[N - 1].Y, relativeMousePos.X, relativeMousePos.Y, null);
            }
        }

    }
}
