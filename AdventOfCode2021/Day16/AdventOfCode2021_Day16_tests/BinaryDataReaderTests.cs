using AdventOfCode2021_Day16;
using Xunit;

namespace AdventOfCode2021_Day16_tests
{
    public class BinaryDataReaderTests
    {
        private readonly BinaryDataReader _sut;

        public BinaryDataReaderTests()
        {
            _sut = new BinaryDataReader();
        }

        [Fact]
        public void ReadAllOfSingleWord()
        {
            var data = new byte[]
            {
                0b0101
            };
            byte result = _sut.GetBinaryData(data, 0, 4);
            
            Assert.Equal(0b0101, result);
        }
        
        [Fact]
        public void ReadPartOfSingleWord()
        {
            var data = new byte[]
            {
                0b0101
            };
            byte result = _sut.GetBinaryData(data, 1, 2);
            
            Assert.Equal(0b10, result);
        }
        
        [Fact]
        public void ReadPartOfSingleWord2()
        {
            var data = new byte[]
            {
                0b0101
            };
            byte result = _sut.GetBinaryData(data, 0, 2);
            
            Assert.Equal(0b01, result);
        }
        
        [Fact]
        public void ReadPartOfSingleWord3()
        {
            var data = new byte[]
            {
                0b0101
            };
            byte result = _sut.GetBinaryData(data, 0, 3);
            
            Assert.Equal(0b010, result);
        }
        
        [Fact]
        public void ReadPartOfSingleWord4()
        {
            var data = new byte[]
            {
                0b0101
            };
            byte result = _sut.GetBinaryData(data, 3, 1);
            
            Assert.Equal(0b1, result);
        }
        
        [Fact]
        public void ReadPartOfSingleWord5()
        {
            var data = new byte[]
            {
                0b0101
            };
            byte result = _sut.GetBinaryData(data, 2, 1);
            
            Assert.Equal(0b0, result);
        }

        [Fact]
        public void ReadAllOfSecondWord()
        {
            var data = new byte[]
            {
                0b0101,
                0b1010
            };
            var result = _sut.GetBinaryData(data, 4, 4);
            Assert.Equal(0b1010, result);
        }
        
        [Fact]
        public void ReadPartSecondWord()
        {
            var data = new byte[]
            {
                0b0101,
                0b1010
            };
            var result = _sut.GetBinaryData(data, 4, 1);
            Assert.Equal(0b1, result);
        }
        
        [Fact]
        public void ReadPartSecondWord2()
        {
            var data = new byte[]
            {
                0b0101,
                0b1010
            };
            var result = _sut.GetBinaryData(data, 5, 2);
            Assert.Equal(0b01, result);
        }
        
        [Fact]
        public void ReadPartSecondWord3()
        {
            var data = new byte[]
            {
                0b0101,
                0b1010
            };
            var result = _sut.GetBinaryData(data, 6, 2);
            Assert.Equal(0b10, result);
        }
        
        [Fact]
        public void ReadWholeOfWords1And2()
        {
            var data = new byte[]
            {
                0b0101,
                0b1010
            };
            var result = _sut.GetBinaryData(data, 0, 8);
            Assert.Equal(0b01011010, result);
        }
        
        [Fact]
        public void ReadWholeOfWords2And3()
        {
            var data = new byte[]
            {
                0b0101,
                0b1010,
                0b1100,
                0b0011
            };
            var result = _sut.GetBinaryData(data, 4, 8);
            Assert.Equal(0b10101100, result);
        }
        
        [Fact]
        public void ReadPartsOfWordsComplex()
        {
            var data = new byte[]
            {
                0b0101,
                0b0101,
                0b0101,
                0b0101
            };
            var index = 0;
            var r1 = _sut.GetBinaryData(data, index, 3);
            index += 3;
            var r2 = _sut.GetBinaryData(data, index, 3);
            index += 3;
            var r3 = _sut.GetBinaryData(data, index, 5);
            index += 5;
            var r4 = _sut.GetBinaryData(data, index, 5);
            index += 5;
            
            Assert.Equal(0b010, r1);
            Assert.Equal(0b101, r2);
            Assert.Equal(0b01010, r3);
            Assert.Equal(0b10101, r4);
        }
    }
}