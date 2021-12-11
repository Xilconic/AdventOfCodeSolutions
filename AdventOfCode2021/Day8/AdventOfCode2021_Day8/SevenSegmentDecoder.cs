using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021_Day8
{
    internal class SevenSegmentDecoder
    {
        private HashSet<char> _one;
        private HashSet<char> _seven;
        private HashSet<char> _four;
        private HashSet<char> _eight;

        private List<HashSet<char>> _ambiguousTwoAndThreeAndFive = new(3);
        private List<HashSet<char>> _ambiguousZeroAndSixAndNine = new(3);
        
        private HashSet<char> _two;
        private HashSet<char> _six;
        private HashSet<char> _three;

        private HashSet<char> _five;
        private HashSet<char> _nine;
        private HashSet<char> _zero;
        
        public SevenSegmentDecoder(IEnumerable<string> signals)
        {
            PerformPhaseOneDecode(signals);
            PerformPhaseTwoDecode();
            PerformPhaseThreeDecode();
        }
        
        private void PerformPhaseOneDecode(IEnumerable<string> signals)
        {
            foreach (string signal in signals)
            {
                var signalAsHashset = new HashSet<char>(signal);
                switch (signal.Length)
                {
                    case 2:
                        _one = signalAsHashset;
                        break;
                    case 3:
                        _seven = signalAsHashset;
                        break;
                    case 4:
                        _four = signalAsHashset;
                        break;
                    case 7:
                        _eight = signalAsHashset;
                        break;
                    case 5:
                        _ambiguousTwoAndThreeAndFive.Add(signalAsHashset);
                        break;
                    case 6:
                        _ambiguousZeroAndSixAndNine.Add(signalAsHashset);
                        break;
                    default: throw new NotImplementedException();
                }
            }
        }

        private void PerformPhaseTwoDecode()
        {
            // The element from '_ambiguousTwoAndThreeAndFive' that intersects with '_four' on 2 elements, can be assigned to '_two':
            foreach (HashSet<char> ambiguousSignal in _ambiguousTwoAndThreeAndFive)
            {
                if (_four.Intersect(ambiguousSignal).Count() == 2)
                {
                    _two = ambiguousSignal;
                    break;
                }
            }
            _ambiguousTwoAndThreeAndFive.Remove(_two);
            
            // The element from '_ambiguousZeroAndSixAndNine' that does not fully contain '_one', can be assigned to '_six':
            foreach (HashSet<char> ambiguousSignal in _ambiguousZeroAndSixAndNine)
            {
                if (!ContainsFully(ambiguousSignal, _one))
                {
                    _six = ambiguousSignal;
                    break;
                }
            }
            _ambiguousZeroAndSixAndNine.Remove(_six);
            
            // The element from '_ambiguousTwoAndThreeAndFive' that fully contains '_seven', can be assigned to '_three':
            foreach (HashSet<char> ambiguousSignal in _ambiguousTwoAndThreeAndFive)
            {
                if (ContainsFully(ambiguousSignal, _seven))
                {
                    _three = ambiguousSignal;
                    break;
                }
            }
            _ambiguousTwoAndThreeAndFive.Remove(_three);
        }

        private void PerformPhaseThreeDecode()
        {
            // After PerformPhaseTwoDecode(), _ambiguousTwoAndThreeAndFive only contains '_five':
            _five = _ambiguousTwoAndThreeAndFive[0];
            _ambiguousTwoAndThreeAndFive = null;
            
            // After PerformPhaseTwoDecode(), _ambiguousZeroAndSixAndNine only contains 9 and 0.
            // The element that fully contains '_four' can be assigned to '_nine'.
            // And therefore the other value will be '_zero':
            foreach (HashSet<char> ambiguousSignal in _ambiguousZeroAndSixAndNine)
            {
                if (ContainsFully(ambiguousSignal, _four))
                {
                    _nine = ambiguousSignal;
                }
                else
                {
                    _zero = ambiguousSignal;
                }
            }
            _ambiguousZeroAndSixAndNine = null;
        }

        public int Decode(string value)
        {
            if (IsEqual(_zero, value)) return 0;
            if (IsEqual(_one, value)) return 1;
            if (IsEqual(_two, value)) return 2;
            if (IsEqual(_three, value)) return 3;
            if (IsEqual(_four, value)) return 4;
            if (IsEqual(_five, value)) return 5;
            if (IsEqual(_six, value)) return 6;
            if (IsEqual(_seven, value)) return 7;
            if (IsEqual(_eight, value)) return 8;
            return 9;
        }

        private bool IsEqual(HashSet<char> source, string target)
        {
            return source.Count == target.Length && !source.Except(target).Any();
        }

        private static bool ContainsFully(HashSet<char> reference, IEnumerable<char> test) => test.All(reference.Contains);
    }
}