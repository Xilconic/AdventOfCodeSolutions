using System;
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
            // Note: All basins are enclosed by the borders of the heightmap and '9' values.
            HeightMap heightmap = HeightMap.FromFile(filename);
            var answer = heightmap.GetBasins()
                .OrderByDescending(b => b.Size)
                .Take(3)
                .Aggregate(1, (aggregate, basin) => aggregate * basin.Size);
            
            Console.WriteLine("What do you get if you multiply together the sizes of the three largest basins?");
            Console.WriteLine(answer);
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