using System;

namespace AdventOfCode2021_Day15
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
            var map = RiskMap.FromFile(filename);
            var pathFinder = new Pathfinder(map);
            Traversal leastRiskyPath = pathFinder.GetLeastRiskyPath();
            
            Console.WriteLine("What is the lowest total risk of any path from the top left to the bottom right?");
            Console.WriteLine(leastRiskyPath.RiskScore);
        }

        private static void SolvePuzzlePart2(string filename)
        {
            var map = RiskMap.FromFile(filename);
            var fullSizeMap = map.CreateFullMap();
            var pathFinder = new Pathfinder(fullSizeMap);
            Traversal leastRiskyPath = pathFinder.GetLeastRiskyPath(); // Note very fast any more :/
            
            Console.WriteLine("What is the lowest total risk of any path from the top left to the bottom right?");
            Console.WriteLine(leastRiskyPath.RiskScore);
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