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

        public RiskMap CreateFullMap()
        {
            var originalSizeX = _riskMap.GetLength(0);
            var originalSizeY = _riskMap.GetLength(1);
            var fullRiskMap = new int[originalSizeX * 5, originalSizeY * 5];
            for (int mapBlockX = 0; mapBlockX < 5; mapBlockX++)
            {
                for (int mapBlockY = 0; mapBlockY < 5; mapBlockY++)
                {
                    for (int sourceX = 0; sourceX < originalSizeX; sourceX++)
                    {
                        for (int sourceY = 0; sourceY < originalSizeY; sourceY++)
                        {
                            fullRiskMap[
                                mapBlockX * originalSizeX + sourceX,
                                mapBlockY * originalSizeY + sourceY
                            ] = GetMapBlockValue(_riskMap[sourceX, sourceY], mapBlockX, mapBlockY);
                        }
                    }
                }
            }

            return new RiskMap(fullRiskMap);
        }

        private int GetMapBlockValue(
            int originalvalue,
            int mapBlockX,
            int mapBlockY)
        {
            var newValue = originalvalue + mapBlockX + mapBlockY;
            while (newValue > 9)
            {
                newValue -= 9;
            }

            return newValue;
        }
    }
}