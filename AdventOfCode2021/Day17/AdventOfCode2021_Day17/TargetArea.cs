using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021_Day17
{
    internal record TargetArea(int Xmin, int Xmax, int Ymin, int Ymax)
    {
        public static TargetArea FromFile(string filename)
        {
            var line = File.ReadLines(filename).First();
            var match = Regex.Match(line, @"target area: x=(?<xMin>-?\d+)..(?<xMax>-?\d+), y=(?<yMin>-?\d+)..(?<yMax>-?\d+)");
            var xMin = int.Parse(match.Groups["xMin"].Value);
            var xMax = int.Parse(match.Groups["xMax"].Value);
            var yMin = int.Parse(match.Groups["yMin"].Value);
            var yMax = int.Parse(match.Groups["yMax"].Value);
            
            Debug.Assert(xMin < xMax);
            Debug.Assert(yMin < yMax);

            return new TargetArea(xMin, xMax, yMin, yMax);
        }
    }
}