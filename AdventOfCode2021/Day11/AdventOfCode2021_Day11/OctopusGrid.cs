using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021_Day11
{
    internal class OctopusGrid
    {
        private int _numberOfFlashes = 0;
        
        private readonly int[,] _octopusEnergyLevels;

        private OctopusGrid(int[,] octopusEnergyLevels)
        {
            _octopusEnergyLevels = octopusEnergyLevels;
        }

        public static OctopusGrid FromFile(string filename)
        {
            var sizeX = 0;
            var energyLevelDataRows = new List<int[]>();
            foreach (string line in File.ReadLines(filename))
            {
                if (sizeX == 0)
                {
                    sizeX = line.Length;
                }

                energyLevelDataRows.Add(line.Select(character => int.Parse((string)character.ToString())).ToArray());
            }

            var energyLevels = new int[sizeX, energyLevelDataRows.Count];
            for (int y = 0; y < energyLevelDataRows.Count; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    energyLevels[x, y] = energyLevelDataRows[y][x];
                }
            }

            return new OctopusGrid(energyLevels);
        }

        public int GetNumberOfFlashes() => _numberOfFlashes;

        public void SimulateStep()
        {
            var octopiToFlash = new HashSet<Octopus>();
            for (int x = 0; x < _octopusEnergyLevels.GetLength(0); x++)
            {
                for (int y = 0; y < _octopusEnergyLevels.GetLength(1); y++)
                {
                    _octopusEnergyLevels[x,y] += 1;
                    
                    if (_octopusEnergyLevels[x, y] > 9)
                    {
                        octopiToFlash.Add(new Octopus(x, y));
                    }
                }
            }

            var octopiThatFlashed = new HashSet<Octopus>();
            while (octopiToFlash.Count > 0)
            {
                var octopus = octopiToFlash.First();

                foreach (Octopus adjacentOctopus in GetAdjacentOctopi(octopus))
                {
                    _octopusEnergyLevels[adjacentOctopus.X,adjacentOctopus.Y] += 1;
                    
                    if (_octopusEnergyLevels[adjacentOctopus.X,adjacentOctopus.Y] > 9 &&
                        !octopiToFlash.Contains(adjacentOctopus) &&
                        !octopiThatFlashed.Contains(adjacentOctopus))
                    {
                        octopiToFlash.Add(adjacentOctopus);
                    }
                }

                octopiToFlash.Remove(octopus);
                octopiThatFlashed.Add(octopus);
            }

            foreach (Octopus octopus in octopiThatFlashed)
            {
                _octopusEnergyLevels[octopus.X, octopus.Y] = 0;
                _numberOfFlashes += 1;
            }
        }

        private IEnumerable<Octopus> GetAdjacentOctopi(Octopus octopus)
        {
            if (octopus.X > 0)
            {
                var xOfLeftOctopus = octopus.X - 1;
                yield return octopus with { X = xOfLeftOctopus };
                if (octopus.Y > 0)
                {
                    yield return octopus with { X = xOfLeftOctopus, Y = octopus.Y - 1 };
                }

                if (octopus.Y < _octopusEnergyLevels.GetLength(1) - 1)
                {
                    yield return octopus with { X = xOfLeftOctopus, Y = octopus.Y + 1 };
                }
            }

            if (octopus.X < _octopusEnergyLevels.GetLength(0) - 1)
            {
                var xOfRightOctopus = octopus.X + 1;
                yield return octopus with { X = xOfRightOctopus };
                if (octopus.Y > 0)
                {
                    yield return octopus with { X = xOfRightOctopus, Y = octopus.Y - 1 };
                }

                if (octopus.Y < _octopusEnergyLevels.GetLength(1) - 1)
                {
                    yield return octopus with { X = xOfRightOctopus, Y = octopus.Y + 1 };
                }
            }

            if (octopus.Y > 0)
            {
                yield return octopus with { Y = octopus.Y - 1 };
            }

            if (octopus.Y < _octopusEnergyLevels.GetLength(1) - 1)
            {
                yield return octopus with { Y = octopus.Y + 1 };
            }
        }

        private record Octopus(int X, int Y);
    }
}