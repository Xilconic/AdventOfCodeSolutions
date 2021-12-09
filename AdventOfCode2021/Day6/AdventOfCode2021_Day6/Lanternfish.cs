namespace AdventOfCode2021_Day6
{
    internal class Lanternfish
    {
        private Lanternfish(int age)
        {
            Age = age;
        }

        public bool WantsToSpawnBabyLanternfish => Age == -1;
        public int Age { get; private set; }

        public static Lanternfish CreatePreexistingWithAge(int age) => new(age);

        public void ProcessAging()
        {
            Age -= 1;
        }

        public Lanternfish CreateOffspring()
        {
            Age = 6;
            return new Lanternfish(8);
        }
    }
}