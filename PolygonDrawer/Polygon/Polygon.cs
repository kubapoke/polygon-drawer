namespace PolygonDrawer
{
    internal class Polygon
    {
        public Polygon() { }

        public static readonly int Eps = 5;
        private List<Point> Points = new List<Point>();
        private List<Line> Lines = new List<Line>();
        private bool IsClosed = false;

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

        public bool AddCreationPoint(int x, int y)
        {
            if (IsClosed) return false;

            if (N >= 3 && Points[0].InBounds(x, y))
            {
                Line line = new Line(Points[N - 1], Points[0]);
                Points[N - 1].L2 = Points[0].L1 = line;
                Lines.Add(line);

                IsClosed = true;
            }
            else
            {
                Points.Add(new Point(x, y));
                if (N >= 2)
                {
                    Line line = new Line(Points[N - 2], Points[N - 1]);
                    Points[N - 2].L2 = Points[N - 1].L1 = line;
                    Lines.Add(line);
                }
            }

            return !IsClosed;
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

        private Point MovePointToLine(Line l, Point p)
        {
            float lineVecX = l.P2.X - l.P1.X;
            float lineVecY = l.P2.Y - l.P1.Y;

            float pointVecX = p.X - l.P1.X;
            float pointVecY = p.Y - l.P1.Y;

            float lineLenSq = lineVecX * lineVecX + lineVecY * lineVecY;
            float t = (pointVecX * lineVecX + pointVecY * lineVecY) / lineLenSq;

            float closestX = l.P1.X + t * lineVecX;
            float closestY = l.P1.Y + t * lineVecY;

            int roundedX = (int)Math.Round(closestX);
            int roundedY = (int)Math.Round(closestY);

            return new Point(roundedX, roundedY);
        }

        public void AddPoint(int l, Point suggestedPoint)
        {
            suggestedPoint = MovePointToLine(Lines[l], suggestedPoint);

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

        public void MovePolygon(int dx, int dy)
        {
            foreach (var point in Points)
            {
                point.MoveLocationIndependent(dx, dy);
            }
        }

        public Point? GetPointAtLocation(int x, int y)
        {
            return Points.Find(p => p.InBounds(x, y));
        }

        public Line? GetLineAtLocation(int x, int y)
        {
            return Lines.Find(l => l.InBounds(x, y));
        }

        public void Draw(Graphics g, Action<Graphics, int, int, int, int, Color?> drawLineAction,
            System.Drawing.Point relativeMousePos)
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

            if (!IsClosed && N > 0)
            {
                drawLineAction(g, Points[N - 1].X, Points[N - 1].Y, relativeMousePos.X, relativeMousePos.Y, null);
            }
        }

        public void ChangeStateOfAllLines(Line.LineState state)
        {
            foreach (var line in Lines)
            {
                line.ChangeState(state);
            }
        }
    }
}
