using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day9
{
    internal class HeightMap
    {
        private readonly int[,] _heightMap;

        private HeightMap(int[,] heightMap)
        {
            _heightMap = heightMap;
        }
        
        public static HeightMap FromFile(string filename)
        {
            var heightLines = new List<int[]>();
            int maxX = 0;
            foreach (string line in File.ReadLines(filename))
            {
                if (maxX == 0)
                {
                    maxX = line.Length;
                }
                
                heightLines.Add(line.Select(c => int.Parse((string)c.ToString())).ToArray());
            }

            var heightMap = new int[maxX, heightLines.Count];
            for (int y = 0; y < heightLines.Count; y++)
            {
                var heightLine = heightLines[y];
                for (int x = 0; x < heightLine.Length; x++)
                {
                    heightMap[x,y] = heightLine[x];
                }
            }

            return new HeightMap(heightMap);
        }

        public IEnumerable<int> GetLowPointsHeights() => GetLowPoints().Select(p => p.Height);

        private IEnumerable<Point> GetLowPoints() => GetAllPoints().Where(IsLowPoint);

        private IEnumerable<Point> GetAllPoints()
        {
            for (int x = 0; x < _heightMap.GetLength(0); x++)
            {
                for (int y = 0; y < _heightMap.GetLength(1); y++)
                {
                    yield return new Point(x, y, _heightMap[x, y]);
                }
            }
        }

        private bool IsLowPoint(Point p)
        {
            if (IsPointNotOnLeftBound(p) && p.Height >= _heightMap[p.X - 1, p.Y])
            {
                return false;
            }

            if (IsPointNotOnRightBound(p) && p.Height >= _heightMap[p.X + 1, p.Y])
            {
                return false;
            }

            if (IsPointNotOnUpperBound(p) && p.Height >= _heightMap[p.X, p.Y - 1])
            {
                return false;
            }
            
            if (IsPointNotOnLowerBound(p) && p.Height >= _heightMap[p.X, p.Y + 1])
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Basin> GetBasins() => GetLowPoints().Select(GrowBasinFromLowPoint);

        private Basin GrowBasinFromLowPoint(Point lowPoint)
        {
            var basinPoints = new HashSet<Point> { lowPoint };
            var pointsToSpreadFrom = new HashSet<Point> { lowPoint };
            while (pointsToSpreadFrom.Count != 0)
            {
                Point spreader = pointsToSpreadFrom.First();

                // Include point to the left into basin?
                if (IsPointNotOnLeftBound(spreader))
                {
                    var leftPointX = spreader.X - 1;
                    Point candidate = spreader with { X = leftPointX, Height = _heightMap[leftPointX, spreader.Y] };
                    if (CanBeSpreaderPoint(candidate, basinPoints))
                    {
                        pointsToSpreadFrom.Add(candidate);
                    }
                }

                // Include point to the right into basin?
                if (IsPointNotOnRightBound(spreader))
                {
                    var rightPointX = spreader.X + 1;
                    Point candidate = spreader with { X = rightPointX, Height = _heightMap[rightPointX, spreader.Y] };
                    if (CanBeSpreaderPoint(candidate, basinPoints))
                    {
                        pointsToSpreadFrom.Add(candidate);
                    }
                }

                // Include point above into basin?
                if (IsPointNotOnUpperBound(spreader))
                {
                    var upperPointY = spreader.Y - 1;
                    Point candidate = spreader with { Y = upperPointY, Height = _heightMap[spreader.X, upperPointY] };
                    if (CanBeSpreaderPoint(candidate, basinPoints))
                    {
                        pointsToSpreadFrom.Add(candidate);
                    }
                }

                // Include point below into basin?
                if (IsPointNotOnLowerBound(spreader))
                {
                    var lowerPointY = spreader.Y + 1;
                    Point candidate = spreader with { Y = lowerPointY, Height = _heightMap[spreader.X, lowerPointY] };
                    if (CanBeSpreaderPoint(candidate, basinPoints))
                    {
                        pointsToSpreadFrom.Add(candidate);
                    }
                }

                basinPoints.Add(spreader);
                pointsToSpreadFrom.Remove(spreader);
            }

            var basins = new Basin(basinPoints.Count);
            return basins;
        }

        private static bool CanBeSpreaderPoint(Point candidate, HashSet<Point> basinPoints)
        {
            return candidate.Height != 9 && !basinPoints.Contains(candidate);
        }

        private static bool IsPointNotOnLeftBound(Point p) => p.X > 0;
        private bool IsPointNotOnRightBound(Point p) => p.X < _heightMap.GetLength(0) - 1;
        
        private static bool IsPointNotOnUpperBound(Point p) => p.Y > 0;
        private bool IsPointNotOnLowerBound(Point p) => p.Y < _heightMap.GetLength(1) - 1;
        
        private record Point(int X, int Y, int Height);
    }
}