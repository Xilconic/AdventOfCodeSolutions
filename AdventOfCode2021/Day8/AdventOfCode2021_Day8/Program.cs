using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021_Day8
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
            var count = 0;
            foreach (string lines in File.ReadLines(filename))
            {
                Match match = Regex.Match(lines,
                    @"\|\s(?<output1>\w+)\s(?<output2>\w+)\s(?<output3>\w+)\s(?<output4>\w+)");
                string output1 = match.Groups["output1"].Value;
                string output2 = match.Groups["output2"].Value;
                string output3 = match.Groups["output3"].Value;
                string output4 = match.Groups["output4"].Value;

                if (IsUnambiguousDriverSignal(output1))
                {
                    count += 1;
                }
                if (IsUnambiguousDriverSignal(output2))
                {
                    count += 1;
                }
                if (IsUnambiguousDriverSignal(output3))
                {
                    count += 1;
                }
                if (IsUnambiguousDriverSignal(output4))
                {
                    count += 1;
                }
            }
            
            Console.WriteLine("In the output values, how many times do digits 1, 4, 7, or 8 appear?");
            Console.WriteLine(count);
        }

        private static bool IsUnambiguousDriverSignal(string sevenSegmentDisplayDriverValue)
        {
            switch (sevenSegmentDisplayDriverValue.Length)
            {
                case 2:
                case 3:
                case 4:
                case 7:
                    return true;
                default: return false;
            }
        }

        private static void SolvePuzzlePart2(string filename)
        {
            long sumOfOutputValues = 0;
            foreach (string lines in File.ReadLines(filename))
            {
                var tokenizedInput = Regex.Matches(lines, @"\w+");
                // Note: tokenizedInput contains 10 driver signals, followed by 4 outputs to be decoded:
                IEnumerable<string> driverSignals = tokenizedInput.Select(m => m.Value).Take(10);
                var decoder = new SevenSegmentDecoder(driverSignals);

                for (int i = 10; i < tokenizedInput.Count; i++)
                {
                    int decodedNumber = decoder.Decode(tokenizedInput[i].Value);
                    switch (i)
                    {
                        case 10: // Thousands number
                            decodedNumber *= 1000;
                            break;
                        case 11: // Hundreds number
                            decodedNumber *= 100;
                            break;
                        case 12: // tens numbers
                            decodedNumber *= 10;
                            break;
                    }
                    sumOfOutputValues += decodedNumber;
                }
            }
            
            Console.WriteLine("What do you get if you add up all of the output values?");
            Console.WriteLine(sumOfOutputValues);
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