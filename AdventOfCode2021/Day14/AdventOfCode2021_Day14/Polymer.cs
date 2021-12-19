using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021_Day14
{
    internal class Polymer
    {
        private readonly string _sequence;
        
        public Polymer(string initialState)
        {
            _sequence = initialState;
        }

        public int CountMostCommonElement() =>
            _sequence
                .GroupBy(c => c)
                .Select(group => group.Count())
                .OrderByDescending(count => count)
                .First();

        public int CountLeastCommonElement() =>
            _sequence
                .GroupBy(c => c)
                .Select(group => group.Count())
                .OrderBy(count => count)
                .First();

        public IReadOnlyList<PolymerPair> GetPairs() => GetPairsStream().ToArray();

        private IEnumerable<PolymerPair> GetPairsStream()
        {
            for (int i = 1; i < _sequence.Length; i++)
            {
                yield return new PolymerPair(_sequence.Substring(i-1, 2));
            }
        }

        public static Polymer StitchTogether(IReadOnlyList<PolymerPair> pairs)
        {
            var stateBuilder = new StringBuilder();
            foreach (PolymerPair pair in pairs)
            {
                stateBuilder.Append(stateBuilder.Length == 0
                    ? pair.GetFullCode()
                    : pair.GetOptionalInsertionWithEndOfCode()
                );
            }

            return new Polymer(stateBuilder.ToString());
        }
    }
}