using System.Collections.Generic;

namespace AdventOfCode2021_Day15
{
    internal class Traversal
    {
        private readonly Stack<Point2D> _trace;

        public Traversal(Stack<Point2D> trace, int riskScore)
        {
            _trace = trace;
            RiskScore = riskScore;
        }

        public int RiskScore { get; }
    }
}