using System;

namespace AdventOfCode2021_Day14
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
            var polymerizationEquipment = PolymerizationEquipment.FromFile(filename);

            for (int i = 0; i < 10; i++)
            {
                polymerizationEquipment.PerformPairInsertionStep();
            }

            Polymer polymer = polymerizationEquipment.GetPolymer();
            
            var numberOfMostCommonElement = polymer.CountMostCommonElement();
            var numberOfLeastCommonElement = polymer.CountLeastCommonElement();
            
            Console.WriteLine("What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?");
            Console.WriteLine(numberOfMostCommonElement - numberOfLeastCommonElement);
        }
        
        private static void SolvePuzzlePart2(string filename)
        {
            // Note: Due to exponential growth of polymer, a different datastructure is needed to represent it than the actual sequence.
            // For this puzzle, the actual sequence of the polymer is irrelevant. Only the pairs matter.
            // Assuming worst case of all 26 characters being possible, then 676 pairs exist in worst case.
            // A way better representation size than ~N*2^40, which is ~1.1e12.
            var polymerizationEquipment = EfficientPolymerizationEquipment.FromFile(filename);

            for (int i = 0; i < 40; i++)
            {
                polymerizationEquipment.PerformPairInsertionStep();
            }

            var numberOfMostCommonElement = polymerizationEquipment.CountMostCommonElement();
            var numberOfLeastCommonElement = polymerizationEquipment.CountLeastCommonElement();
            
            Console.WriteLine("What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?");
            Console.WriteLine(numberOfMostCommonElement - numberOfLeastCommonElement);
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