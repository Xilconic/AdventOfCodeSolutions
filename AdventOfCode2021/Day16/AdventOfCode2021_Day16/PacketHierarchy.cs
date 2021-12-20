using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day16
{
    internal class PacketHierarchy
    {
        private readonly Packet _rootPacket;

        private PacketHierarchy(Packet rootPacket)
        {
            _rootPacket = rootPacket;
        }

        public static PacketHierarchy FromFile(string filename)
        {
            var binaryDataReader = new BinaryDataReader();

            var packageHierarchyInHexadecimal = File.ReadLines(filename).First();
            var packageHierarchyInBinaryWords = packageHierarchyInHexadecimal
                .Select(c => byte.Parse(c.ToString(), NumberStyles.HexNumber))
                .ToArray();

            var currentIndex = 0;
            int ReadNextBits(int length)
            {
                int data = binaryDataReader.GetBinaryData(packageHierarchyInBinaryWords, currentIndex, length);
                currentIndex += length;
                return data;
            }

            Packet ReadNextPacket()
            {
                int version = ReadNextBits(3);
                int typeId = ReadNextBits(3);
                switch (typeId)
                {
                    case 4:
                    {
                        int literalChunk;
                        long literalValue = 0;
                        
                        var safetyCounter = 1; // Exists to safeguard that `long` is large enough to store data
                        do
                        {
                            Debug.Assert(safetyCounter < 16);

                            literalChunk = ReadNextBits(5);;

                            literalValue <<= 4;
                            literalValue += literalChunk & 0b01111;

                            safetyCounter++;
                        } while ((literalChunk & 0b10000) > 0);

                        var package = new LiteralValuePacket(version, typeId, literalValue);
                        return package;
                    }
                    default:
                    {
                        var lengthTypeId = ReadNextBits(1);
                        switch (lengthTypeId)
                        {
                            case 0:
                            {
                                int length = ReadNextBits(15);
                                var packages = new List<Packet>();
                                var savedIndex = currentIndex;
                                do
                                {
                                    packages.Add(ReadNextPacket());
                                } while ((currentIndex - savedIndex) != length);
                                var package = new OperatorPacket(version, typeId, packages);
                                return package;
                            }
                            case 1:
                            {
                                int length = ReadNextBits(11);
                                var packages = new List<Packet>();
                                for (int i = 0; i < length; i++)
                                {
                                    packages.Add(ReadNextPacket());
                                }
                                var package = new OperatorPacket(version, typeId, packages);
                                return package;
                            }
                        }
                        break;
                    }
                }

                throw new NotImplementedException();
            }

            var rootPackage = ReadNextPacket();

            return new PacketHierarchy(rootPackage);
        }

        public IEnumerable<Packet> GetAllPacketsRecursively() => 
            GetPacketsRecursively(_rootPacket);

        private static IEnumerable<Packet> GetPacketsRecursively(Packet p)
        {
            yield return p;
            if (p is OperatorPacket operatorPackage)
            {
                foreach (Packet subPacket in operatorPackage.SubPackets.SelectMany(GetPacketsRecursively))
                {
                    yield return subPacket;
                }
            }
        }
    }
}