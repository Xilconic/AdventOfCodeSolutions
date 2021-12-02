using System;
using System.IO;

namespace AdventOfCode2021.Day1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException(
                    "Program must be supplied exactly 1 argument, being the puzzle input file.",
                    nameof(args));
            }

            bool isFirstMeasurement = true;
            int previousDepthMeasurement = default;
            int numberOfTimesCurrentMeasurementWasHigherThanPrevious = 0;
            foreach (string line in File.ReadLines(args[0]))
            {
                if (!int.TryParse(line, out int depthMeasurement))
                {
                    throw new FormatException($"Puzzle input should have a single integer value per line in the file. Instead, read unexpected {line}.");
                }

                if (isFirstMeasurement)
                {
                    isFirstMeasurement = false;
                }
                else
                {
                    if (depthMeasurement > previousDepthMeasurement)
                    {
                        numberOfTimesCurrentMeasurementWasHigherThanPrevious += 1;
                    }
                }

                previousDepthMeasurement = depthMeasurement;
            }
            
            Console.WriteLine("How many measurements are larger than the previous measurement?");
            Console.WriteLine(numberOfTimesCurrentMeasurementWasHigherThanPrevious);
        }
    }
}