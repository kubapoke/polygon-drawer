using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonDrawer
{
    internal class Point
    {
        private static int Counter = 0;
        public readonly int Idx;
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Idx = Counter++;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public static int Eps = 5;

        public int ManhattanDistance(int x, int y)
        {
            return Math.Max(Math.Abs(x - this.X), Math.Abs(y - this.Y));
        }

        public bool InBounds(int x, int y)
        {
            return ManhattanDistance(x, y) <= Eps;
        }

        public static void ResetCounter()
        { 
            Counter = 0; 
        }
    }

    internal class Line
    {
        private static int Counter = 0;
        public readonly int Idx;

        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.Idx = Counter++;
        }

        public Point p1 { get; set; }
        public Point p2 { get; set; }

        public static void ResetCounter()
        {
            Counter = 0;
        }
    }
    internal class Polygon
    {
        public Polygon() 
        {
            Point.ResetCounter();
            Line.ResetCounter();
        }

        public List<Point> Points = new List<Point>();
        public List<Line> Lines = new List<Line>();

        public int N { get {  return Points.Count; } }

        public bool InBounds(int x, int y)
        {
            int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;

            foreach(var point in Points)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }

            return minX <= x && minY <= y && maxX >= x && maxY >= y;
        }
    }
}
