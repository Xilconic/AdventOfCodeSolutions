using System;

namespace AdventOfCode2021_Day4
{
    class Program
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

        private static void SolvePuzzlePart1(string filename)
        {
            var bingoGame = BingoGame.InitializeFromFile(filename);
            bingoGame.PlayGameUntilFirstWinningBingoCard();
            
            var finalScore = bingoGame.GetSumOfAllUnmarkedNumbersOfWinningBingoCard() * bingoGame.GetLastCalledNumber();
            Console.WriteLine("What will your final score be if you choose that board?");
            Console.WriteLine(finalScore);
        }
        
        private static void SolvePuzzlePart2(string filename)
        {
            var bingoGame = BingoGame.InitializeFromFile(filename);
            bingoGame.PlayGameUntilAllBingoCardsWon();
            
            var finalScore = bingoGame.GetSumOfAllUnmarkedNumbersOfWinningBingoCard() * bingoGame.GetLastCalledNumber();
            Console.WriteLine("What will your final score be if you choose that board?");
            Console.WriteLine(finalScore);
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