using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day6
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
            var ocean = new Ocean();
            foreach (string line in File.ReadLines(filename))
            {
                IEnumerable<Lanternfish> lanternfishesToInitializeOceanWith = GetLanternfishes(line);
                ocean.InsertLanternfish(lanternfishesToInitializeOceanWith);
            }

            for (int i = 0; i < 80; i++)
            {
                ocean.SimulateDay();
            }
            
            Console.WriteLine("How many lanternfish would there be after 80 days?");
            Console.WriteLine(ocean.GetNumberOfLanternfish());
        }

        private static void SolvePuzzlePart2(string filename)
        {
            // Note: For such a high number of simulation days, we're exceeding int.MaxValue.
            // Therefore need to make use of more efficient reasoning about populations...
            var ocean = new LifecycleBasedOcean();
            foreach (string line in File.ReadLines(filename))
            {
                IEnumerable<Lanternfish> lanternfishesToInitializeOceanWith = GetLanternfishes(line);
                ocean.InsertLanternfish(lanternfishesToInitializeOceanWith);
            }

            for (int i = 0; i < 256; i++)
            {
                ocean.SimulateDay();
            }
            
            Console.WriteLine("How many lanternfish would there be after 256 days?");
            Console.WriteLine(ocean.GetNumberOfLanternfish());
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
        
        private static IEnumerable<Lanternfish> GetLanternfishes(string line)
        {
            return line
                .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Select(Lanternfish.CreatePreexistingWithAge);
        }
    }
}