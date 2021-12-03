namespace AdventOfCode2021.Day2
{
    public class AimSensitiveSubmarine : ISubmarine
    {
        private int _horizontalPosition = 0;
        private int _depth = 0;
        private int _aim = 0;
        
        public void GoForward(int distance)
        {
            _horizontalPosition += distance;
            _depth += _aim * distance;
        }

        public void GoDeeper(int distance) => _aim += distance;
        public void GoHigher(int distance) => _aim -= distance;

        public long GetDepthTimesForwardDistance() => _depth * _horizontalPosition;
    }
}