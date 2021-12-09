using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021_Day6
{
    internal class LifecycleBasedOcean
    {
        private long _numberOfFishInBabyStage1 = 0;
        private long _numberOfFishInBabyStage2 = 0;
        private readonly LanternfishLifecycleBucket[] _lifecycleBuckets;

        public LifecycleBasedOcean()
        {
            _lifecycleBuckets = Enumerable.Range(0, 7)
                .Select(cycleAge => new LanternfishLifecycleBucket(cycleAge))
                .ToArray();
        }

        public void InsertLanternfish(IEnumerable<Lanternfish> fishes)
        {
            foreach (IGrouping<int, Lanternfish> fishWithSameAge in fishes.GroupBy(f => f.Age))
            {
                _lifecycleBuckets[fishWithSameAge.Key].IncreasePopulation(fishWithSameAge.Count());
            }
        }

        public void SimulateDay()
        {
            long numberOfNewborns = 0;
            
            foreach (LanternfishLifecycleBucket bucket in _lifecycleBuckets)
            {
                bucket.ProcessAging();
                if (bucket.WantsToSpawnBabyLanternfish)
                {
                    numberOfNewborns += bucket.CreateOffspring();
                }
            }
            
            LanternfishLifecycleBucket bucketAtAge6 = _lifecycleBuckets.First(b => b.Age == 6);

            bucketAtAge6.IncreasePopulation(_numberOfFishInBabyStage2);                
            _numberOfFishInBabyStage2 = _numberOfFishInBabyStage1;
            _numberOfFishInBabyStage1 = numberOfNewborns;
        }

        public long GetNumberOfLanternfish()
        {
            return _lifecycleBuckets.Sum(b => b.Count)
                   + _numberOfFishInBabyStage1
                   + _numberOfFishInBabyStage2;
        }
    }
}