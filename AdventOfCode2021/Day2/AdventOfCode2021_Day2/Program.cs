using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Day2
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
            ISubmarine submarine = new Submarine();
            switch (ParsePuzzleSolvingMode(args[1]))
            {
                case PuzzleSolvingMode.Part1:
                    submarine = new Submarine();
                    break;
                case PuzzleSolvingMode.Part2:
                    submarine = new AimSensitiveSubmarine();
                    break;
            }

            foreach (string submarineControlInstruction in File.ReadLines(filename))
            {
                Match match = Regex.Match(submarineControlInstruction, @"(?<command>\w+)\s(?<distance>\d+)");
                string command = match.Groups["command"].Value;
                string distanceAsText = match.Groups["distance"].Value;
                int distance = int.Parse(distanceAsText);
                
                switch (command)
                {
                    case "forward":
                        submarine.GoForward(distance);
                        break;
                    case "down":
                        submarine.GoDeeper(distance);
                        break;
                    case "up":
                        submarine.GoHigher(distance);
                        break;
                }
            }
            
            Console.WriteLine("What do you get if you multiply your final horizontal position by your final depth?");
            Console.WriteLine(submarine.GetDepthTimesForwardDistance());
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