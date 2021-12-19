using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day15
{
    internal class RiskMap
    {
        private readonly int[,] _riskMap;

        private RiskMap(int[,] riskMap)
        {
            _riskMap = riskMap;
            SizeX = _riskMap.GetLength(0);
            SizeY = _riskMap.GetLength(1);
        }

        public int SizeX { get; }
        public int SizeY { get; }

        public static RiskMap FromFile(string filename)
        {
            var sizeX = 0;
            var rows = new List<int[]>();
            foreach (string line in File.ReadLines(filename))
            {
                if (sizeX == 0)
                {
                    sizeX = line.Length;
                }
                rows.Add(line.Select(c => int.Parse((string)c.ToString())).ToArray());
            }

            var riskMap = new int[sizeX, rows.Count];
            for (int y = 0; y < rows.Count; y++)
            {
                var row = rows[y];
                for (int x = 0; x < sizeX; x++)
                {
                    riskMap[x, y] = row[x];
                }
            }
            
            return new RiskMap(riskMap);
        }

        public int GetRiskScore(Point2D point) => 
            _riskMap[point.X, point.Y];
    }
}