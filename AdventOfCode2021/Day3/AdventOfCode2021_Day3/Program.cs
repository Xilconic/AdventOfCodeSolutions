using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace AdventOfCode2021.Day3
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
                    SolvePart1(filename);
                    break;
                case PuzzleSolvingMode.Part2:
                    SolvePart2(filename);
                    break;
            }
        }

        private static void SolvePart1(string filename)
        {
            int[] numberOfOnesPerColumn = null;
            int numberOfLines = 0;
            foreach (string binaryVariable in File.ReadLines(filename))
            {
                // On first line read, we know how much bits there are per line:
                numberOfOnesPerColumn ??= new int[binaryVariable.Length];

                // Count all the 1's per column:
                for (int i = 0; i < binaryVariable.Length; i++)
                {
                    if (binaryVariable[i] == '1')
                    {
                        numberOfOnesPerColumn[i] += 1;
                    }
                }

                numberOfLines += 1;
            }

            // Half the number of lines is the boundary for determining the majority:
            var halfSize = numberOfLines / 2;
            
            // Initialize the variables to 0.
            // Then generate the binary representation by using leftshift + increments.
            uint gamma = 0;
            uint epsilon = 0;
            for (int i = 0; i < numberOfOnesPerColumn.Length; i++)
            {
                gamma <<= 1;
                epsilon <<= 1;
                
                bool onesAreInMajority = numberOfOnesPerColumn[i] > halfSize;
                if (onesAreInMajority)
                {
                    gamma += 1;
                }
                else
                {
                    epsilon += 1;
                }
            }

            // The integers can deal simultaneously with binary and decimal reasoning.
            uint result = gamma * epsilon;
            Console.WriteLine("What is the power consumption of the submarine?");
            Console.WriteLine(result);
        }
        
        private static void SolvePart2(string filename)
        {
            int binarySize = 0;
            var binaryNumbers = new List<string>();
            foreach (string binaryValueAsString in File.ReadLines(filename))
            {
                if (binarySize == 0)
                {
                    binarySize = binaryValueAsString.Length;
                }
                
                binaryNumbers.Add(binaryValueAsString);
            }

            var oxygenGeneratorRating = ProgressiveFilter(binaryNumbers, binarySize, SplittingStrategy.Majority, TiebreakerMode.UseValuesWithOneAtIndex);
            var co2ScrubberRating = ProgressiveFilter(binaryNumbers, binarySize, SplittingStrategy.Minority, TiebreakerMode.UseValuesWithZeroAtIndex);
            
            // The integers can deal simultaneously with binary and decimal reasoning.
            uint lifeSupportRating = oxygenGeneratorRating * co2ScrubberRating;
            Console.WriteLine("What is the life support rating of the submarine?");
            Console.WriteLine(lifeSupportRating);
        }

        private static uint ProgressiveFilter(
            IReadOnlyList<string> binaryNumbers,
            int binarySize,
            SplittingStrategy splittingStrategy,
            TiebreakerMode tieBreakerStrategy)
        {
            for (int i = 0; i < binarySize; i++)
            {
                var binaryValuesWithOnesAtIndex = new List<string>(binaryNumbers.Count);
                var binaryValuesWithZerosAtIndex = new List<string>(binaryNumbers.Count);
                
                // Are we done filtering? If yes, early return:
                if (binaryNumbers.Count == 1)
                {
                    return Convert.ToUInt32(binaryNumbers[0], 2);
                }
                 
                // Split the source collection in number that have a 1 or 0 at the given index:
                foreach (string binaryNumber in binaryNumbers)
                {
                    if (binaryNumber[i] == '1')
                    {
                        binaryValuesWithOnesAtIndex.Add(binaryNumber);
                    }
                    else
                    {
                        binaryValuesWithZerosAtIndex.Add(binaryNumber);
                    }
                }
                
                // Select which split to keep and continue filtering using the next index:
                if (binaryValuesWithOnesAtIndex.Count == binaryValuesWithZerosAtIndex.Count)
                {
                    switch (tieBreakerStrategy)
                    {
                        case TiebreakerMode.UseValuesWithOneAtIndex:
                            binaryNumbers = binaryValuesWithOnesAtIndex;
                            break;
                        case TiebreakerMode.UseValuesWithZeroAtIndex:
                            binaryNumbers = binaryValuesWithZerosAtIndex;
                            break;
                        default: throw new InvalidEnumArgumentException(nameof(tieBreakerStrategy), (int)tieBreakerStrategy, typeof(TiebreakerMode));
                    }
                }
                else
                {
                    int halfSize = binaryNumbers.Count / 2;
                    switch (splittingStrategy)
                    {
                        case SplittingStrategy.Majority:
                            binaryNumbers = binaryValuesWithOnesAtIndex.Count > binaryValuesWithZerosAtIndex.Count ? binaryValuesWithOnesAtIndex : binaryValuesWithZerosAtIndex;
                            break;
                        case SplittingStrategy.Minority:
                            binaryNumbers = binaryValuesWithOnesAtIndex.Count < binaryValuesWithZerosAtIndex.Count ? binaryValuesWithOnesAtIndex : binaryValuesWithZerosAtIndex;
                            break;
                        default: throw new InvalidEnumArgumentException(nameof(splittingStrategy), (int)splittingStrategy, typeof(SplittingStrategy));
                    }
                }
            }

            return Convert.ToUInt32(binaryNumbers[0], 2);
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

        private enum SplittingStrategy
        {
            Majority,
            Minority
        }

        private enum TiebreakerMode
        {
            UseValuesWithOneAtIndex,
            UseValuesWithZeroAtIndex,
        }
    }
}