using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2021_Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException(
                    "Program must be supplied exactly 1 arguments, being the puzzle input file.",
                    nameof(args));
            }

            var submarine = new Submarine();

            foreach (string submarineControlInstruction in File.ReadLines(args[0]))
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
    }
}