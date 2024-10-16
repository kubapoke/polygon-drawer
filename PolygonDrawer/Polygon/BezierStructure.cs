namespace PolygonDrawer
{
    internal class BezierStructure
    {
        private Point[] points = new Point[4];

        public BezierStructure(Line line)
        {
            points[0] = line.P1;
            points[3] = line.P2;

            Line prevLine = line.P1.L1!, nextLine = line.P2.L2!;

            points[1] = new Point(2 * line.P1.X - prevLine.P1.X, 2 * line.P1.Y - prevLine.P1.Y);
            points[2] = new Point(2 * line.P2.X - nextLine.P2.X, 2 * line.P2.Y - nextLine.P2.Y);
        }

        public void Draw(Graphics g, Action<Graphics, int, int, int, int, Color?> drawLineAction)
        {
            drawLineAction(g, points[0].X, points[0].Y, points[1].X, points[1].Y, null);
            drawLineAction(g, points[1].X, points[1].Y, points[2].X, points[2].Y, null);
            drawLineAction(g, points[2].X, points[2].Y, points[3].X, points[3].Y, null);

            System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
            System.Drawing.Rectangle r1 = new System.Drawing.Rectangle(points[1].X - Polygon.Eps, points[1].Y - Polygon.Eps, 2 * Polygon.Eps, 2 * Polygon.Eps);
            System.Drawing.Rectangle r2 = new System.Drawing.Rectangle(points[2].X - Polygon.Eps, points[2].Y - Polygon.Eps, 2 * Polygon.Eps, 2 * Polygon.Eps);

            g.FillEllipse(brush, r1);
            g.FillEllipse(brush, r2);
        }
    }
}
