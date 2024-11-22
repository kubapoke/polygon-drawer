using System.Numerics;

namespace PolygonDrawer
{
    internal class Drawer
    {
        private static System.Drawing.SolidBrush Brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        private static void DrawPixel(Graphics g, int x, int y, System.Drawing.Color color)
        {
            g.FillRectangle(new System.Drawing.SolidBrush(color), x, y, 1, 1);
        }
        private static void DrawLineLow(Graphics g, int x1, int y1, int x2, int y2, System.Drawing.Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int yi = 1;

            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            int D = (2 * dy) - dx;
            int y = y1;

            for (int x = x1; x <= x2; x++)
            {
                DrawPixel(g, x, y, color);
                if (D > 0)
                {
                    y += yi;
                    D = D + (2 * (dy - dx));
                }
                else
                {
                    D = D + 2 * dy;
                }
            }
        }
        private static void DrawLineHigh(Graphics g, int x1, int y1, int x2, int y2, System.Drawing.Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int xi = 1;

            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            int D = (2 * dx) - dy;
            int x = x1;

            for (int y = y1; y <= y2; y++)
            {
                DrawPixel(g, x, y, color);
                if (D > 0)
                {
                    x += xi;
                    D = D + (2 * (dx - dy));
                }
                else
                {
                    D = D + 2 * dx;
                }
            }
        }
        public static void BresenhamDrawLine(Graphics g, int x1, int y1, int x2, int y2, System.Drawing.Color? color = null)
        {
            System.Drawing.Color _color = color ?? System.Drawing.Color.Black;

            if (x1 == x2 && y1 == y2)
                return;

            if (Math.Abs(y2 - y1) < Math.Abs(x2 - x1))
            {
                if (x1 > x2)
                    DrawLineLow(g, x2, y2, x1, y1, _color);
                else
                    DrawLineLow(g, x1, y1, x2, y2, _color);
            }
            else
            {
                if (y1 > y2)
                    DrawLineHigh(g, x2, y2, x1, y1, _color);
                else
                    DrawLineHigh(g, x1, y1, x2, y2, _color);
            }
        }

        private static void DrawDottedLineLow(Graphics g, int x1, int y1, int x2, int y2, System.Drawing.Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int count = 0;
            int yi = 1;

            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            int D = (2 * dy) - dx;
            int y = y1;

            for (int x = x1; x <= x2; x++)
            {
                if ((count++) % 6 <= 2)
                    DrawPixel(g, x, y, color);
                if (D > 0)
                {
                    y += yi;
                    D = D + (2 * (dy - dx));
                }
                else
                {
                    D = D + 2 * dy;
                }
            }
        }
        private static void DrawDottedLineHigh(Graphics g, int x1, int y1, int x2, int y2, System.Drawing.Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int count = 0;
            int xi = 1;

            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            int D = (2 * dx) - dy;
            int x = x1;

            for (int y = y1; y <= y2; y++)
            {
                if ((count++) % 6 <= 2)
                    DrawPixel(g, x, y, color);
                if (D > 0)
                {
                    x += xi;
                    D = D + (2 * (dx - dy));
                }
                else
                {
                    D = D + 2 * dx;
                }
            }
        }
        public static void BresenhamDrawDottedLine(Graphics g, int x1, int y1, int x2, int y2, System.Drawing.Color? color = null)
        {
            System.Drawing.Color _color = color ?? System.Drawing.Color.Gray;

            if (x1 == x2 && y1 == y2)
                return;

            if (Math.Abs(y2 - y1) < Math.Abs(x2 - x1))
            {
                if (x1 > x2)
                    DrawDottedLineLow(g, x2, y2, x1, y1, _color);
                else
                    DrawDottedLineLow(g, x1, y1, x2, y2, _color);
            }
            else
            {
                if (y1 > y2)
                    DrawDottedLineHigh(g, x2, y2, x1, y1, _color);
                else
                    DrawDottedLineHigh(g, x1, y1, x2, y2, _color);
            }
        }

        public static void LibraryDrawLine(Graphics g, int x1, int y1, int x2, int y2, System.Drawing.Color? color = null)
        {
            System.Drawing.Color _color = color ?? System.Drawing.Color.Black;

            g.DrawLine(new System.Drawing.Pen(_color), x1, y1, x2, y2);
        }

        public static void DrawTextNextToLine(Graphics g, int x1, int y1, int x2, int y2, string text, System.Drawing.Color? color = null)
        {
            Color textColor = color ?? System.Drawing.Color.Blue;
            Brush brush = new SolidBrush(textColor);
            Font font = new Font("Arial", 12);

            float midX = (x1 + x2) / 2f;
            float midY = (y1 + y2) / 2f;

            int dx = x2 - x1;
            int dy = y2 - y1;

            const int offset = 10;
            float textX = midX, textY = midY;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                textY += (dy > 0) ? offset : -offset;
            }
            else
            {
                textX += (dx > 0) ? offset : -offset;
            }

            g.DrawString(text, font, brush, new PointF(textX, textY));
        }

        public static void DrawTextNextToPoint(Graphics g, Point p, string text, Color? color = null)
        {
            Color textColor = color ?? Color.Blue;

            using Brush brush = new SolidBrush(textColor);
            using Font font = new Font("Arial", 12);

            const int offsetX = 8;
            const int offsetY = -15;

            g.DrawString(text, font, brush, new PointF((float)(p.X + offsetX), (float)(p.Y + offsetY)));
        }

        public static void DrawBezierCurve(Graphics g, Vector2 V0, Vector2 V1, Vector2 V2, Vector2 V3)
        {
            double dx = V0.X - V3.X;
            double dy = V0.Y - V3.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);

            if (length <= 0)
                return;

            length = Math.Max(length, 50.0);

            float d = 1 / (float)length;

            Vector2 A0 = V0;
            Vector2 A1 = 3 * (V1 - V0);
            Vector2 A2 = 3 * (V2 - (2 * V1) + V0);
            Vector2 A3 = V3 - (3 * V2) + (3 * V1) - V0;

            Vector2 P = A0, prevP = A0;
            Vector2 dP = A3 * d * d * d + A2 * d * d + A1 * d;
            Vector2 d2P = 6 * A3 * d * d * d + 2 * A2 * d * d;
            Vector2 d3P = 6 * A3 * d * d * d;

            for (int i = 0; i < length; i++)
            {
                prevP = P;

                P = P + dP;
                dP = dP + d2P;
                d2P = d2P + d3P;

                int x1 = (int)prevP.X;
                int y1 = (int)prevP.Y;
                int x2 = (int)P.X;
                int y2 = (int)P.Y;

                BresenhamDrawLine(g, x1, y1, x2, y2);
            }
        }
    }
}
