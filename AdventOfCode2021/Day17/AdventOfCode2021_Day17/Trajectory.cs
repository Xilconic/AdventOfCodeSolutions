namespace AdventOfCode2021_Day17
{
    internal class Trajectory
    {
        private readonly int _maxHeight;

        public Trajectory(int maxHeight)
        {
            _maxHeight = maxHeight;
        }

        public int GetMaxY() => _maxHeight;
    }
}