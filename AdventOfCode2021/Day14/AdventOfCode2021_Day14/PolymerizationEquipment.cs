using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021_Day14
{
    internal class PolymerizationEquipment
    {
        private Polymer _polymer;
        private readonly IReadOnlyDictionary<string, string> _pairInsertionRules;

        private PolymerizationEquipment(
            Polymer polymer,
            IReadOnlyDictionary<string, string> pairInsertionRules)
        {
            _polymer = polymer;
            _pairInsertionRules = pairInsertionRules;
        }

        public static PolymerizationEquipment FromFile(string filename)
        {
            string initialState = null;
            var pairInsertionRules = new Dictionary<string, string>();
            foreach (string line in File.ReadLines(filename)
                         .Where(l => !string.IsNullOrWhiteSpace(l)))
            {
                if (initialState is null)
                {
                    initialState = line;
                }
                else
                {
                    var match = Regex.Match(line, @"(?<pair>\w\w) -> (?<insertion>\w)");
                    var pair = match.Groups["pair"].Value;
                    var insertion = match.Groups["insertion"].Value;

                    pairInsertionRules[pair] = insertion;
                }
            }

            return new PolymerizationEquipment(new Polymer(initialState), pairInsertionRules);
        }

        public void PerformPairInsertionStep()
        {
            var pairs = _polymer.GetPairs().ToArray();
            foreach (PolymerPair pair in pairs)
            {
                if (_pairInsertionRules.TryGetValue(pair.Code, out string insertion))
                {
                    pair.InsertInMiddle(insertion);
                }
            }

            _polymer = Polymer.StitchTogether(pairs);
        }

        public Polymer GetPolymer() => _polymer;
    }
}