using System;
using System.IO;

namespace AdventOfCode2021.Day1
{
    public class Program
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

        private static void SolvePuzzlePart1(string filename)
        {
            try
            {
                var numberOfTimesCurrentMeasurementWasHigherThanPrevious =
                    File.ReadLines(filename)
                        .ToIntegers()
                        .CountNumberOfTimesCurrentElementIsHigherThanPreviousElement();
                
                Console.WriteLine("How many measurements are larger than the previous measurement?");
                Console.WriteLine(numberOfTimesCurrentMeasurementWasHigherThanPrevious);
            }
            catch (Exception e)
            {
                throw new FormatException($"Puzzle input should exist and have a single integer value per line in the file.", e);
            }
        }

        private static void SolvePuzzlePart2(string filename)
        {
            try
            {
                var numberOfTimesCurrentMeasurementWasHigherThanPrevious =
                    File.ReadLines(filename)
                        .ToIntegers()
                        .ToSumsOfSlidingWindowOfSize3()
                        .CountNumberOfTimesCurrentElementIsHigherThanPreviousElement();
                
                Console.WriteLine("How many measurements are larger than the previous measurement using a sliding window of size 3?");
                Console.WriteLine(numberOfTimesCurrentMeasurementWasHigherThanPrevious);
            }
            catch (Exception e)
            {
                throw new FormatException($"Puzzle input should exist and have a single integer value per line in the file.", e);
            }
        }
    }
}