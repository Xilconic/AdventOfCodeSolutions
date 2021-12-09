namespace AdventOfCode2021_Day6
{
    internal class LanternfishLifecycleBucket
    {
        public LanternfishLifecycleBucket(int cycleAge)
        {
            Age = cycleAge;
        }

        public long Count { get; private set; } = 0;
        public bool WantsToSpawnBabyLanternfish => Age == -1;
        public int Age { get; private set; }

        public void IncreasePopulation(long count)
        {
            Count += count;
        }

        public void ProcessAging()
        {
            Age -= 1;
        }

        public long CreateOffspring()
        {
            Age = 6;
            return Count;
        }
    }
}