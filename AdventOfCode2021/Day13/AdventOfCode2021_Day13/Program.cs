using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2021_Day13
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
            (PointsCollection points, IReadOnlyList<FoldInstruction> foldInstructions) = ParseFile(filename);
            var paper = TransparentPaper.From(points);

            foreach (FoldInstruction instruction in foldInstructions.Take(1))
            {
                paper.FoldAccordingTo(instruction);
            }
            
            Console.WriteLine("How many dots are visible after completing just the first fold instruction on your transparent paper?");
            Console.WriteLine(paper.GetVisiblePoints().Count);
        }

        private static void SolvePuzzlePart2(string filename)
        {
            (PointsCollection points, IReadOnlyList<FoldInstruction> foldInstructions) = ParseFile(filename);
            var paper = TransparentPaper.From(points);

            foreach (FoldInstruction instruction in foldInstructions)
            {
                paper.FoldAccordingTo(instruction);
            }
            
            Console.WriteLine("What code do you use to activate the infrared thermal imaging camera system?");
            Console.WriteLine(paper.ToString());
        }
        
        private static (PointsCollection, IReadOnlyList<FoldInstruction>) ParseFile(string filename)
        {
            var readingMode = ReadingMode.Points;
            var points = new PointsCollection();
            var instructions = new List<FoldInstruction>();
            
            foreach (string line in File.ReadLines(filename))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    readingMode = ReadingMode.FoldInstructions;
                }
                else
                {
                    switch (readingMode)
                    {
                        case ReadingMode.Points:
                            points.Add(Point2D.Parse(line));
                            break;
                        case ReadingMode.FoldInstructions:
                            instructions.Add(FoldInstruction.Parse(line));
                            break;
                    }
                }
            }

            return new(points, instructions);
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
        
        private enum ReadingMode
        {
            Points,
            FoldInstructions
        }
    }

    internal record Point2D(int X, int Y)
    {
        public static Point2D Parse(string pointAsString)
        {
            var coordinateValuesAsString = pointAsString.Split(",");
            return new Point2D(int.Parse(coordinateValuesAsString[0]), int.Parse(coordinateValuesAsString[1]));
        }
    }

    internal record FoldInstruction(FoldAxis Axis, int Coordinate)
    {
        public static FoldInstruction Parse(string line)
        {
            var match = Regex.Match(line, @"fold along (?<axis>x|y)=(?<coordinate>\d+)");
            var coordinate = int.Parse(match.Groups["coordinate"].Value);
            FoldAxis foldAxis;
            switch (match.Groups["axis"].Value)
            {
                case "x":
                    foldAxis = FoldAxis.Vertical;
                    break;
                case "y":
                    foldAxis = FoldAxis.Horizontal;
                    break;
                default: throw new NotImplementedException();
            }

            return new FoldInstruction(foldAxis, coordinate);
        }
    }

    internal class PointsCollection
    {
        private int _maxX;
        private int _maxY;
        private readonly List<Point2D> _points = new();
        
        public void Add(Point2D point)
        {
            _points.Add(point);
            if (point.X > _maxX)
            {
                _maxX = point.X;
            }

            if (point.Y > _maxY)
            {
                _maxY = point.Y;
            }
        }

        public bool[,] ToPointLocationMap()
        {
            var map = new bool[_maxX + 1, _maxY + 1];
            foreach (Point2D point in _points)
            {
                map[point.X, point.Y] = true;
            }

            return map;
        }
    }
    
    internal class TransparentPaper
    {
        private bool[,] _points;

        private TransparentPaper(bool[,] points)
        {
            _points = points;
        }

        public static TransparentPaper From(PointsCollection points) => 
            new(points.ToPointLocationMap());

        public void FoldAccordingTo(FoldInstruction instruction)
        {
            switch (instruction.Axis)
            {
                case FoldAxis.Horizontal:
                {
                    var foldY = instruction.Coordinate;
                    var newPoints = new bool[_points.GetLength(0), foldY];
                    for (int x = 0; x < _points.GetLength(0); x++)
                    {
                        for (int y = 0; y < foldY; y++)
                        {
                            newPoints[x, y] = _points[x, y];
                        }
                        
                        for (int y = foldY+1; y < _points.GetLength(1); y++)
                        {
                            newPoints[x, foldY - (y - foldY)] |= _points[x, y];
                        }
                    }

                    _points = newPoints;
                    break;
                }
                case FoldAxis.Vertical:
                {
                    var foldX = instruction.Coordinate;
                    var newPoints = new bool[foldX, _points.GetLength(1)];
                    for (int y = 0; y < _points.GetLength(1); y++)
                    {
                        for (int x = 0; x < foldX; x++)
                        {
                            newPoints[x, y] = _points[x, y];
                        }
                        
                        for (int x = foldX+1; x < _points.GetLength(0); x++)
                        {
                            newPoints[foldX - (x - foldX), y] |= _points[x, y];
                        }
                    }
                    
                    _points = newPoints;
                    break;
                }
            }
        }

        public IReadOnlyCollection<Point2D> GetVisiblePoints() => 
            GetPoints().ToArray();

        private IEnumerable<Point2D> GetPoints()
        {
            for (int x = 0; x < _points.GetLength(0); x++)
            {
                for (int y = 0; y < _points.GetLength(1); y++)
                {
                    if (_points[x, y])
                    {
                        yield return new Point2D(x, y);
                    }
                }
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for (int y = 0; y < _points.GetLength(1); y++)
            {
                for (int x = 0; x < _points.GetLength(0); x++)
                {
                    stringBuilder.Append(_points[x, y] ? "#" : ".");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }

    internal enum FoldAxis { Horizontal, Vertical }
}