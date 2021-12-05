using System;
using System.Linq;

namespace AdventOfCode2021_Day4
{
    internal class BingoCardParser
    {
        private int[,] _numberGrid = new int[5, 5];
        private int _currentIndex = 0;
        
        public void PartialParse(string line)
        {
            var rowNumbers = line
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            for (int j = 0; j < 5; j++)
            {
                _numberGrid[_currentIndex, j] = rowNumbers[j];
            }
            _currentIndex += 1;
        }

        public BingoCard BuildFromParsedData()
        {
            if (_currentIndex != 5)
            {
                throw new InvalidOperationException("Should have partially parsed exactly 5 rows before building BingoCard.");
            }
            
            return new BingoCard(_numberGrid);
        }

        public void Reset()
        {
            _currentIndex = 0;
            _numberGrid = new int[5, 5];
        }
    }
}