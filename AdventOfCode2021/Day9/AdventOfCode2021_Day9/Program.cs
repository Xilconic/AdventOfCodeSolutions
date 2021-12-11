using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException(
                    "Program must be supplied exactly 2 arguments; First being the puzzle input file, second being the part number in the range [1,2].",
                    nameof(args));
            }

            string filename = args[0];
            switch (ParsePuzzleSolvingMode(args[1]))
            {
                case PuzzleSolvingMode.Part1:
                    SolvePuzzlePart1(filename);
                    break;
                case PuzzleSolvingMode.Part2:
                    SolvePuzzlePart2(filename);
                    break;
            }
        }

        private static void SolvePuzzlePart1(string filename)
        {
            HeightMap heightmap = HeightMap.FromFile(filename); 
            var sumOfRiskLevels = heightmap.GetLowPointsHeights().Select(height => height + 1).Sum();
            Console.WriteLine("What is the sum of the risk levels of all low points on your heightmap?");
            Console.WriteLine(sumOfRiskLevels);
        }

        private static void SolvePuzzlePart2(string filename)
        {
            throw new NotImplementedException();
        }

        private static PuzzleSolvingMode ParsePuzzleSolvingMode(string puzzlePartNumber)
        {
            if (!int.TryParse(puzzlePartNumber, out int number))
                throw new ArgumentException($"2nd argument only supports values in the range [1,2]. Received {puzzlePartNumber}", "args[1]");
            
            return number switch
            {
                1 => PuzzleSolvingMode.Part1,
                2 => PuzzleSolvingMode.Part2,
                _ => throw new ArgumentException($"2nd argument only supports values in the range [1,2]. Received {number}", "args[1]")
            };
        }
    }

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
                
                heightLines.Add(line.Select(c => int.Parse(c.ToString())).ToArray());
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

        public IEnumerable<int> GetLowPointsHeights()
        {
            return GetAllPoints()
                .Where(IsLowPoint)
                .Select(p => p.Height);
        }

        private IEnumerable<Point> GetAllPoints()
        {
            for (int X = 0; X < _heightMap.GetLength(0); X++)
            {
                for (int Y = 0; Y < _heightMap.GetLength(1); Y++)
                {
                    yield return new Point(X, Y, _heightMap[X, Y]);
                }
            }
        }

        private bool IsLowPoint(Point p)
        {
            if (p.X > 0 && p.Height >= _heightMap[p.X - 1, p.Y])
            {
                return false;
            }

            if (p.X < _heightMap.GetLength(0) - 1 && p.Height >= _heightMap[p.X + 1, p.Y])
            {
                return false;
            }

            if (p.Y > 0 && p.Height >= _heightMap[p.X, p.Y - 1])
            {
                return false;
            }
            
            if (p.Y < _heightMap.GetLength(1) - 1 && p.Height >= _heightMap[p.X, p.Y + 1])
            {
                return false;
            }

            return true;
        }

        private record Point(int X, int Y, int Height);
    }
}