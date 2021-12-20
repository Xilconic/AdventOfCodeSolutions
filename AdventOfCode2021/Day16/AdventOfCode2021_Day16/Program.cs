using System;
using System.Collections.Generic;
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
            var packetHierarchy = PacketHierarchy.FromFile(filename);
            var packets = packetHierarchy.GetAllPacketsRecursively();
            var sumOfVersionNumbers = packets.Sum(packet => packet.VersionNumber);
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

    internal class OperatorPacket : Packet
    {
        public OperatorPacket(int version, int typeId, IReadOnlyCollection<Packet> packages)
            : base(version, typeId)
        {
            SubPackets = packages;
        }

        public IReadOnlyCollection<Packet> SubPackets { get; }
    }

    internal class LiteralValuePacket : Packet
    {
        private readonly long _literalValue;

        public LiteralValuePacket(int version, int typeId, long literalValue)
            : base(version, typeId)
        {
            _literalValue = literalValue;
        }
    }

    internal abstract class Packet
    {
        protected Packet(
            int versionNumber,
            int typeId)
        {
            VersionNumber = versionNumber;
            TypeId = typeId;
        }

        public int VersionNumber { get; }
        public int TypeId { get; }
    }
}