using System.Text.RegularExpressions;

namespace AdventOfCode2021_Day5
{
    internal class Line
    {
        public Point Point1 { get; }
        public Point Point2 { get; }

        private Line(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;

            if (point1.X > point2.X)
            {
                MinX = point2.X;
                MaxX = point1.X;
            }
            else
            {
                MinX = point1.X;
                MaxX = point2.X;
            }
            
            if (point1.Y > point2.Y)
            {
                MinY = point2.Y;
                MaxY = point1.Y;
            }
            else
            {
                MinY = point1.Y;
                MaxY = point2.Y;
            }

            IsHorizontal = point1.Y == point2.Y;
            IsVertical = point1.X == point2.X;
            IsDiagonal = (MaxX - MinX) == (MaxY - MinY);
        }

        public static Line Parse(string lineAsText)
        {
            Match match = Regex.Match(lineAsText, @"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)");
            var point1 = new Point(int.Parse(match.Groups["x1"].Value), int.Parse(match.Groups["y1"].Value));
            var point2 = new Point(int.Parse(match.Groups["x2"].Value), int.Parse(match.Groups["y2"].Value));

            return new Line(point1, point2);
        }
        
        public int MinX { get; }
        public int MaxX { get; }
        public int MinY { get; }
        public int MaxY { get; }
        
        public bool IsHorizontal { get; }
        public bool IsVertical { get; }
        public bool IsDiagonal { get; }
    }
}