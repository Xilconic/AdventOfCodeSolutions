namespace AdventOfCode2021.Day2
{
    internal class Submarine : ISubmarine
    {
        private int _horizontalPosition = 0;
        private int _depth = 0;
        
        public void GoForward(int distance) => _horizontalPosition += distance;
        public void GoDeeper(int distance) => _depth += distance;
        public void GoHigher(int distance) => _depth -= distance;

        public long GetDepthTimesForwardDistance() => _depth * _horizontalPosition;
    }
}