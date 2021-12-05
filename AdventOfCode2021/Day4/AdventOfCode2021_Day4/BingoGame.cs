using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day4
{
    internal class BingoGame
    {
        private readonly Queue<int> _numbers;
        private readonly IReadOnlyCollection<BingoCard> _bingoCards;
        private BingoCard _winningBingoCard = null;
        private int _lastCalledNumber;

        public BingoGame(
            IEnumerable<int> numbers,
            IReadOnlyCollection<BingoCard> bingoCards)
        {
            _numbers = new Queue<int>(numbers);
            _bingoCards = bingoCards;
        }
        
        public static BingoGame InitializeFromFile(string filename)
        {
            IReadOnlyCollection<int> numbers = null;
            var bingoCardParser = new BingoCardParser();
            var bingoCards = new List<BingoCard>();

            int rowNumber = 0;
            foreach (string line in File.ReadLines(filename))
            {
                rowNumber++;
                
                if (numbers is null)
                {
                    numbers = line.Split(
                            ',',
                            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(int.Parse)
                        .ToArray();
                }
                else if(string.IsNullOrWhiteSpace(line))
                {
                    if (rowNumber != 2)
                    {
                        bingoCards.Add(bingoCardParser.BuildFromParsedData());
                        bingoCardParser.Reset();                        
                    }
                }
                else
                {
                    bingoCardParser.PartialParse(line);
                }
            }

            return new BingoGame(numbers, bingoCards);
        }
        
        public void PlayGameUntilFirstWinningBingoCard()
        {
            while (_winningBingoCard is null)
            {
                // Assumption: all games will always result in a winner before emptying out '_numbers'.
                _lastCalledNumber = _numbers.Dequeue();
                foreach (BingoCard bingoCard in _bingoCards)
                {
                    bingoCard.DaubFor(_lastCalledNumber);
                }
                
                // Assumption: There is always only 1 first winner.
                _winningBingoCard = _bingoCards.FirstOrDefault(bingoCard => bingoCard.HasWon);
            }
        }

        public int GetSumOfAllUnmarkedNumbersOfWinningBingoCard() => _winningBingoCard.GetSumOfAllUndaubedNumbers();
        public int GetLastCalledNumber() => _lastCalledNumber;
    }
}