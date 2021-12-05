using System;

namespace AdventOfCode2021_Day4
{
    internal class BingoCard
    {
        private readonly int[,] _numberGrid;
        private readonly bool[,] _marks = new bool[5, 5]; // All initialized as false by default, by design.

        public BingoCard(int[,] numberGrid)
        {
            if (numberGrid.GetLength(0) != 5 &&
                numberGrid.GetLength(1) != 5)
            {
                throw new ArgumentException("Supplied grid must be 5x5.", nameof(numberGrid));
            }

            _numberGrid = numberGrid;
        }

        public void DaubFor(int numberToCallOut)
        {
            for (int rowIndex = 0; rowIndex < 5; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 5; columnIndex++)
                {
                    if (_numberGrid[rowIndex, columnIndex] == numberToCallOut)
                    {
                        _marks[rowIndex, columnIndex] = true;
                        CheckIfCardWon();
                        
                        // Assumption: All numbers are unique on the Bingo Card
                        return;
                    }
                }
            }
        }

        private void CheckIfCardWon()
        {
            for (int rowIndex = 0; rowIndex < 5; rowIndex++)
            {
                // Enable early exit to not check other values in row wastefully:
                bool couldBeWinningRow = true;
                for (int columnIndex = 0; columnIndex < 5 && couldBeWinningRow; columnIndex++)
                {
                    if (!_marks[rowIndex, columnIndex])
                    {
                        couldBeWinningRow = false;
                    }
                }

                if (couldBeWinningRow)
                {
                    HasWon = true;
                    return;
                }
            }

            for (int columnIndex = 0; columnIndex < 5; columnIndex++)
            {
                // Enable early exit to not check other values in column wastefully:
                bool couldBeWinningColumn = true;
                for (int rowIndex = 0; rowIndex < 5 && couldBeWinningColumn; rowIndex++)
                {
                    if (!_marks[rowIndex, columnIndex])
                    {
                        couldBeWinningColumn = false;
                    }
                }
                
                if (couldBeWinningColumn)
                {
                    HasWon = true;
                    return;
                }
            }
        }

        public bool HasWon { get; private set; } = false;

        public int GetSumOfAllUndaubedNumbers()
        {
            var sum = 0;
            for (int rowIndex = 0; rowIndex < 5; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 5; columnIndex++)
                {
                    if (!_marks[rowIndex, columnIndex])
                    {
                        sum += _numberGrid[rowIndex, columnIndex];
                    }
                }
            }

            return sum;
        }
    }
}