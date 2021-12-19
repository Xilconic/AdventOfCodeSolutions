using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021_Day14
{
    internal class EfficientPolymerizationEquipment
    {
        private readonly string _startElement, _endElement;
        private IReadOnlyDictionary<string, long> _pairCounts;
        private readonly IReadOnlyDictionary<string, string> _pairInsertionRules;

        private EfficientPolymerizationEquipment(
            IReadOnlyDictionary<string, long> pairCounts,
            IReadOnlyDictionary<string, string> pairInsertionRules,
            string startElement, string endElement)
        {
            _pairCounts = pairCounts;
            _pairInsertionRules = pairInsertionRules;
            _startElement = startElement;
            _endElement = endElement;
        }

        public static EfficientPolymerizationEquipment FromFile(string filename)
        {
            string startElement = null;
            string endElement = null;
            Dictionary<string, long> pairCounts = null;
            var pairInsertionRules = new Dictionary<string, string>();
            foreach (string line in File.ReadLines(filename)
                         .Where(l => !string.IsNullOrWhiteSpace(l)))
            {
                if (pairCounts is null)
                {
                    pairCounts = new Dictionary<string, long>();
                    startElement = line[0].ToString();
                    endElement = line[^1].ToString();
                    
                    for (int i = 1; i < line.Length; i++)
                    {
                        var pair = line.Substring(i-1, 2);
                        if (pairCounts.TryGetValue(pair, out long count))
                        {
                            pairCounts[pair] = count+1;
                        }
                        else
                        {
                            pairCounts[pair] = 1;
                        }
                    }
                }
                else
                {
                    var match = Regex.Match(line, @"(?<pair>\w\w) -> (?<insertion>\w)");
                    var pair = match.Groups["pair"].Value;
                    var insertion = match.Groups["insertion"].Value;

                    pairInsertionRules[pair] = insertion;
                }
            }

            return new EfficientPolymerizationEquipment(pairCounts, pairInsertionRules, startElement, endElement);
        }

        public void PerformPairInsertionStep()
        {
            var postInsertionPairs = new Dictionary<string, long>();
            foreach (KeyValuePair<string,long> kvp in _pairCounts)
            {
                if (_pairInsertionRules.TryGetValue(kvp.Key, out string insertion))
                {
                    var pair1 = $"{kvp.Key[0]}{insertion}";
                    var pair2 = $"{insertion}{kvp.Key[1]}";
                    AddOrInsertStringCount(postInsertionPairs, pair1, kvp.Value);
                    AddOrInsertStringCount(postInsertionPairs, pair2, kvp.Value);
                }
                else
                {
                    AddOrInsertStringCount(postInsertionPairs, kvp.Key, kvp.Value);
                }
            }

            _pairCounts = postInsertionPairs;
        }

        private static void AddOrInsertStringCount(
            IDictionary<string, long> postInsertionPairs,
            string pair,
            long increment = 1)
        {
            if (postInsertionPairs.TryGetValue(pair, out long count))
            {
                postInsertionPairs[pair] = count + increment;
            }
            else
            {
                postInsertionPairs[pair] = increment;
            }
        }

        public long CountMostCommonElement()
        {
            Dictionary<string, long> elementCounts = GetElementCountsDoubled();
            return elementCounts.Max(kvp => kvp.Value) / 2;
        }

        public long CountLeastCommonElement()
        {
            Dictionary<string, long> elementCounts = GetElementCountsDoubled();
            return elementCounts.Min(kvp => kvp.Value) / 2;
        }
        
        private Dictionary<string, long> GetElementCountsDoubled()
        {
            var elementCounts = new Dictionary<string, long>();
            foreach (var pairCount in _pairCounts)
            {
                AddOrInsertStringCount(elementCounts, pairCount.Key[0].ToString(), pairCount.Value);
                AddOrInsertStringCount(elementCounts, pairCount.Key[1].ToString(), pairCount.Value);
            }

            // Note: At this point all elements are counted double; once for each pair.
            // Except the start and end elements are not counted 2x, because those have no 2 pairs corresponding to them.
            elementCounts[_startElement] += 1;
            elementCounts[_endElement] += 1;
            
            return elementCounts;
        }
    }
}