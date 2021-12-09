using System;
using System.IO;
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

                NumberGuesses number1Guesses = GuessNumber(output1);
                NumberGuesses number2Guesses = GuessNumber(output2);
                NumberGuesses number3Guesses = GuessNumber(output3);
                NumberGuesses number4Guesses = GuessNumber(output4);

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

        private static NumberGuesses GuessNumber(string sevenSegmentDisplayDriverValue)
        {
            switch (sevenSegmentDisplayDriverValue.Length)
            {
                case 2: return NumberGuesses.One;
                case 3: return NumberGuesses.Seven;
                case 4: return NumberGuesses.Four;
                case 5: return NumberGuesses.Two | NumberGuesses.Three | NumberGuesses.Five;
                case 6: return NumberGuesses.Zero | NumberGuesses.Six | NumberGuesses.Nine;
                case 7: return NumberGuesses.Eight;
                default: throw new NotImplementedException();
            }
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

        [Flags]
        public enum NumberGuesses
        {
            Zero = 1, // 6 segments
            One = 2, // 2 segments
            Two = 4, // 5 segments
            Three = 8, // 5 segments
            Four = 16, // 4 segments
            Five = 32, // 5 segments
            Six = 64, // 6 segments
            Seven = 128, // 3 segments
            Eight = 256, // 7 segments
            Nine = 512 // 6 segments
        }
    }
}