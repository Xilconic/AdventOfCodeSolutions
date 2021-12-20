using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day16
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
            var packageHierachy = PackageHierarchy.FromFile(filename);
            var packages = packageHierachy.GetAllPackagesRecursively();
            var sumOfVersionNumbers = packages.Sum(packages => packages.VersionNumber);
            Console.WriteLine("What do you get if you add up the version numbers in all packets?");
            Console.WriteLine(sumOfVersionNumbers);
        }

        private static void SolvePuzzlePart2(string filename)
        {
            throw new NotImplementedException();
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

    internal class PackageHierarchy
    {
        public static PackageHierarchy FromFile(string filename)
        {
            var binaryDataReader = new BinaryDataReader();
            
            var packageHierarchyInHexadecimal = File.ReadLines(filename).First();
            var packageHierarchyInBinaryWords = packageHierarchyInHexadecimal
                .Select(c => byte.Parse(c.ToString(), NumberStyles.HexNumber))
                .ToArray();

            var currentIndex = 0;
            
            byte version = binaryDataReader.GetBinaryData(packageHierarchyInBinaryWords, currentIndex, 3);
            currentIndex += 3;
            byte typeId = binaryDataReader.GetBinaryData(packageHierarchyInBinaryWords, currentIndex, 3);
            currentIndex += 3;
            switch (typeId)
            {
                case 4:
                    byte literalChunk;
                    long literalValue = 0;
                    
                    var safetyCounter = 1; // Exists to safeguard that `long` is large enough to store data
                    do
                    {
                        Debug.Assert(safetyCounter < 16);

                        literalChunk = binaryDataReader.GetBinaryData(packageHierarchyInBinaryWords, currentIndex, 5);
                        currentIndex += 5;

                        literalValue <<= 4;
                        literalValue += literalChunk & 0b01111;

                        safetyCounter++;
                    } while ((literalChunk & 0b10000) > 0);

                    var package = new LiteralValuePackage(version, typeId, literalValue);
                    break;
                default:
                    // TODO: Some kind of nesting stuff
                    break;
            }

            return null;
        }

        public IEnumerable<Package> GetAllPackagesRecursively()
        {
            throw new NotImplementedException();
        }
    }

    internal class LiteralValuePackage : Package
    {
        private readonly long _literalValue;

        public LiteralValuePackage(byte version, byte typeId, long literalValue)
            : base(version, typeId)
        {
            _literalValue = literalValue;
        }
    }

    internal abstract class Package
    {
        protected Package(
            byte versionNumber,
            byte typeId)
        {
            VersionNumber = versionNumber;
            TypeId = typeId;
        }

        public byte VersionNumber { get; }
        public byte TypeId { get; }
    }
}