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
                var lanternfishesToInitializeOceanWith = line.Split(",", StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .Select(age => Lanternfish.CreatePreexistingWithAge(age));
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

    internal class Lanternfish
    {
        private int _age;

        private Lanternfish(int age)
        {
            _age = age;
        }

        public bool WantsToSpawnBabyLanternfish => _age == -1;

        public static Lanternfish CreatePreexistingWithAge(int age) => new(age);

        public void ProcessAging()
        {
            _age -= 1;
        }

        public Lanternfish CreateOffspring()
        {
            _age = 6;
            return new Lanternfish(8);
        }
    }

    internal class Ocean
    {
        private readonly List<Lanternfish> _lanternfishes = new();
        
        public void InsertLanternfish(IEnumerable<Lanternfish> lanternfishes)
        {
            _lanternfishes.AddRange(lanternfishes);
        }

        public void SimulateDay()
        {
            var numberOfLanternfishAtStartOfDay = GetNumberOfLanternfish();
            for (int i = 0; i < numberOfLanternfishAtStartOfDay; i++)
            {
                Lanternfish fish = _lanternfishes[i];
                fish.ProcessAging();
                if (fish.WantsToSpawnBabyLanternfish)
                {
                    _lanternfishes.Add(fish.CreateOffspring());
                }
            }
        }

        public int GetNumberOfLanternfish()
        {
            return _lanternfishes.Count;
        }
    }
}