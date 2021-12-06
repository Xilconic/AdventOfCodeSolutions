using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021_Day5
{
    internal class Map
    {
        private readonly int[,] _map;
        
        public Map(int maxX, int maxY)
        {
            _map = new int[maxX+1, maxY+1]; // All values initialized to 0 by default, by design.
        }

        public void DrawLines(IEnumerable<Line> lines)
        {
            foreach (Line line in lines)
            {
                if (line.IsHorizontal)
                {
                    for (int x = line.MinX; x <= line.MaxX; x++)
                    {
                        // Line is horizontal, so MinY == MaxY; Can therefore take any of the 2:
                        _map[x, line.MinY] += 1;
                    }
                }
                else if(line.IsVertical)
                {
                    for (int y = line.MinY; y <= line.MaxY; y++)
                    {
                        // Line is horizontal, so MinX == MaxX; Can therefore take any of the 2:
                        _map[line.MinX, y] += 1;
                    }
                }
                else if (line.IsDiagonal)
                {
                    Point leftMostPoint, rightMostPoint;
                    if (line.Point1.X < line.Point2.X)
                    {
                        leftMostPoint = line.Point1;
                        rightMostPoint = line.Point2;
                    }
                    else
                    {
                        leftMostPoint = line.Point2;
                        rightMostPoint = line.Point1;
                    }

                    int x = leftMostPoint.X;
                    int y = leftMostPoint.Y;
                    if (leftMostPoint.Y < rightMostPoint.Y)
                    {
                        for (; x <= rightMostPoint.X; x++, y++)
                        {
                            _map[x, y] += 1;
                        }
                    }
                    else
                    {
                        for (; x <= rightMostPoint.X; x++, y--)
                        {
                            _map[x, y] += 1;
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException("Only support drawing vertical and horizontal lines.");
                }
            }
        }
        
        public int GetNumberOfIntersections()
        {
            var numberOfIntersections = 0;
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    if (_map[x, y] > 1)
                    {
                        numberOfIntersections += 1;
                    }
                }
            }

            return numberOfIntersections;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    stringBuilder.Append(_map[x, y] == 0 ? "." : _map[x, y].ToString());
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}