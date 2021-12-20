using System;
using System.Diagnostics;

namespace AdventOfCode2021_Day16
{
    public class BinaryDataReader
    {
        private const int WordSize = 4;

        public byte GetBinaryData(byte[] data, int index, int length)
        {
            Debug.Assert(length > 0);
            Debug.Assert(length <= 8);
            Debug.Assert(index >= 0);
            Debug.Assert(index < data.Length*4);
            
            var arrayIndex = index / WordSize;
            var wordIndex = index % WordSize;
            
            var numberOfBitsRead = 0;
            byte result = 0;
            do
            {
                byte word = data[arrayIndex];
                
                int numberOfBitsFromCurrentWord = DetermineNumberOfBitsReadFromCurrentWord(length, numberOfBitsRead, wordIndex);
                byte mask = GetMask(numberOfBitsFromCurrentWord, wordIndex);
                byte binaryData = ReadBinaryDataChunk(word, mask, wordIndex, numberOfBitsFromCurrentWord);
                result = PostPendBits(result, binaryData, numberOfBitsFromCurrentWord);

                arrayIndex += 1;
                wordIndex = 0;
                numberOfBitsRead += numberOfBitsFromCurrentWord;
            } while (numberOfBitsRead != length);
            
            return result;
        }

        private static byte PostPendBits(byte result, byte binaryData, int numberOfBitsFromCurrentWord)
        {
            result <<= numberOfBitsFromCurrentWord;
            result += binaryData;
            return result;
        }

        private static byte ReadBinaryDataChunk(byte word, byte mask, int wordIndex, int numberOfBitsFromCurrentWord)
        {
            int binaryData = (word & mask) >> (WordSize - wordIndex - numberOfBitsFromCurrentWord);
            return (byte)binaryData;
        }

        private static byte GetMask(int numberOfBitsFromCurrentWord, int wordIndex)
        {
            byte mask = numberOfBitsFromCurrentWord switch
            {
                1 => 0b1000,
                2 => 0b1100,
                3 => 0b1110,
                WordSize => 0b1111,
                _ => 0
            };

            mask >>= wordIndex;
            return mask;
        }

        private static int DetermineNumberOfBitsReadFromCurrentWord(int length, int numberOfBitsRead, int wordIndex)
        {
            var numberOfBitsWantedToReadFromCurrentWord = Math.Min(length - numberOfBitsRead, WordSize);
            var numberOfBitsActuallyAvailableFromCurrentWord = WordSize - wordIndex;
            return Math.Min(numberOfBitsWantedToReadFromCurrentWord, numberOfBitsActuallyAvailableFromCurrentWord);
        }
    }
}