using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2021_Day5
{
    class Program
    {
        public static void Main(string[] args)
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
            var maxX = 0;
            var maxY = 0;

            var lines = new List<Line>();
            foreach (string lineAsText in File.ReadLines(filename))
            {
                var line = Line.Parse(lineAsText);
                // Ignore all non horizontal and non vertical lines:
                if (line.IsHorizontal || line.IsVertical)
                {
                    if (line.MaxX > maxX)
                    {
                        maxX = line.MaxX;
                    }

                    if (line.MaxY > maxY)
                    {
                        maxY = line.MaxY;
                    }

                    lines.Add(line);
                }
            }

            var map = new Map(maxX, maxY);
            map.DrawLines(lines);
            
            int numberOfIntersectionLinePoints = map.GetNumberOfIntersections();
            Console.WriteLine("At how many points do at least two lines overlap?");
            Console.WriteLine(numberOfIntersectionLinePoints);
        }
        
        private static void SolvePuzzlePart2(string filename)
        {
            var maxX = 0;
            var maxY = 0;

            var lines = new List<Line>();
            foreach (string lineAsText in File.ReadLines(filename))
            {
                var line = Line.Parse(lineAsText);
                // Ignore all non horizontal and non vertical lines:
                if (line.IsHorizontal || line.IsVertical || line.IsDiagonal)
                {
                    if (line.MaxX > maxX)
                    {
                        maxX = line.MaxX;
                    }

                    if (line.MaxY > maxY)
                    {
                        maxY = line.MaxY;
                    }

                    lines.Add(line);
                }
            }

            var map = new Map(maxX, maxY);
            map.DrawLines(lines);
            
            int numberOfIntersectionLinePoints = map.GetNumberOfIntersections();
            Console.WriteLine("At how many points do at least two lines overlap?");
            Console.WriteLine(numberOfIntersectionLinePoints);
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
}