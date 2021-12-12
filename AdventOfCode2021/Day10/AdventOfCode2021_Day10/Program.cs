using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day10
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
            var sum = 0;
            foreach (string line in File.ReadLines(filename))
            {
                var stack = new Stack<char>();
                foreach (char character in line)
                {
                    // Assume all 1st characters are always opening braces
                    if (stack.Count == 0)
                    {
                        stack.Push(character);
                    }
                    else
                    {
                        if (IsOpeningNewChunk(character))
                        {
                            stack.Push(character);
                        }
                        else if(IsClosingCurrentChunk(character, stack.Peek()))
                        {
                            stack.Pop();
                        }
                        else
                        {
                            // Corruption detected!
                            int score = GetSyntaxErrorScore(character);
                            sum += score;
                            break;
                        }
                    }
                }
            }
            
            Console.WriteLine("What is the total syntax error score for those errors?");
            Console.WriteLine(sum);
        }

        private static bool IsOpeningNewChunk(char character)
        {
            switch (character)
            {
                case '(':
                case '[':
                case '{':
                case '<':
                    return true;
                default: return false;
            }
        }

        private static bool IsClosingCurrentChunk(char character, char peek) =>
            peek switch
            {
                
                '(' => character == ')',
                '[' => character == ']',
                '{' => character == '}',
                '<' => character == '>',
                _ => throw new NotImplementedException()
            };

        private static int GetSyntaxErrorScore(char character)
        {
            return character switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
                _ => throw new NotImplementedException()
            };
        }

        private static void SolvePuzzlePart2(string filename)
        {
            var sum = 0;
            var sortedScore = new SortedList<long, long>();
            foreach (string line in File.ReadLines(filename))
            {
                bool isCorruptedLine = false;
                var openingBraces = new Stack<char>();
                foreach (char character in line)
                {
                    // Assume all 1st characters are always opening braces
                    if (openingBraces.Count == 0)
                    {
                        openingBraces.Push(character);
                    }
                    else
                    {
                        if (IsOpeningNewChunk(character))
                        {
                            openingBraces.Push(character);
                        }
                        else if(IsClosingCurrentChunk(character, openingBraces.Peek()))
                        {
                            openingBraces.Pop();
                        }
                        else
                        {
                            // Corruption detected!
                            isCorruptedLine = true;
                            break;
                        }
                    }
                }

                if (!isCorruptedLine)
                {
                    long score = 0;
                    foreach (char openingBrace in openingBraces)
                    {
                        score *= 5;
                        score += GetAutocompletionScore(openingBrace);
                    }
                    
                    sortedScore.Add(score, score);
                }
            }
            
            Console.WriteLine("What is the middle score?");
            Console.WriteLine(sortedScore.ElementAt(sortedScore.Count/2).Value);
        }

        private static long GetAutocompletionScore(char openingBrace) =>
            openingBrace switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                '<' => 4,
                _ => throw new NotImplementedException()
            };

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