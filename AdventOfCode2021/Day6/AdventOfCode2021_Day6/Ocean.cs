using System.Collections.Generic;

namespace AdventOfCode2021_Day6
{
    internal class Ocean
    {
        private readonly List<Lanternfish> _lanternfishes = new();
        
        public void InsertLanternfish(IEnumerable<Lanternfish> lanternfishes)
        {
            _lanternfishes.AddRange(lanternfishes);
        }

        public void SimulateDay()
        {
            var numberOfLanternfishAtStartOfDay = GetNumberOfLanternfish();
            for (int i = 0; i < numberOfLanternfishAtStartOfDay; i++)
            {
                Lanternfish fish = _lanternfishes[i];
                fish.ProcessAging();
                if (fish.WantsToSpawnBabyLanternfish)
                {
                    _lanternfishes.Add(fish.CreateOffspring());
                }
            }
        }

        public int GetNumberOfLanternfish()
        {
            return _lanternfishes.Count;
        }
    }
}