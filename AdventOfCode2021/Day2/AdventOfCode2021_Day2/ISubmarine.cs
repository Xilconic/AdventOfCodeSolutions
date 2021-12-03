namespace AdventOfCode2021.Day2
{
    internal interface ISubmarine
    {
        void GoForward(int distance);
        void GoDeeper(int distance);
        void GoHigher(int distance);
        long GetDepthTimesForwardDistance();
    }
}