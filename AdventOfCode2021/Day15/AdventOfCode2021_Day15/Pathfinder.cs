using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021_Day15
{
    /// <summary>
    /// Simple A* algorithm implementation based on a given risk map,
    /// for finding a path with the minimal risk.
    /// </summary>
    internal class Pathfinder
    {
        private readonly RiskMap _map;
        private readonly Dictionary<Point2D, int> _costFromStartPerPoint = new();
        private readonly Dictionary<Point2D, int> _totalCostPerPoint = new();
        private readonly Dictionary<Point2D, Point2D> _optimalPreviousPoint = new();
        private readonly Point2D _startPoint;
        private readonly Point2D _endPoint;

        public Pathfinder(RiskMap map)
        {
            _map = map;
            _startPoint = new Point2D(0, 0);
            _endPoint = new Point2D(_map.SizeX - 1, _map.SizeY - 1);
        }

        public Traversal GetLeastRiskyPath()
        {
            _costFromStartPerPoint[_startPoint] = 0;
            _totalCostPerPoint[_startPoint] = EstimateLowestRiskToGetToTarget(_startPoint, _endPoint);

            var openSet = new HashSet<Point2D>();
            openSet.Add(_startPoint);

            while (openSet.Count > 0)
            {
                // Find lowest cost point in open set:
                var current = openSet
                    .Select(p => new Point2DWithCost(p, _totalCostPerPoint[p]))
                    .OrderBy(pwc => pwc.Cost)
                    .First().Point;
                if (current.Equals(_endPoint))
                {
                    return RecreatePath();
                }

                openSet.Remove(current);

                foreach (Point2D neighbour in GetNeighboursFor(current).Where(p => !p.Equals(_startPoint)))
                {
                    var tentativeCost = _costFromStartPerPoint[current] + _map.GetRiskScore(neighbour);
                    var hasCostFromStartPoint = _costFromStartPerPoint.TryGetValue(neighbour, out int lowestKnownCost);
                    if (!hasCostFromStartPoint ||
                        tentativeCost < lowestKnownCost)
                    {
                        _optimalPreviousPoint[neighbour] = current;
                        _costFromStartPerPoint[neighbour] = tentativeCost;
                        _totalCostPerPoint[neighbour] = tentativeCost + EstimateLowestRiskToGetToTarget(neighbour, _endPoint);
                        openSet.Add(neighbour);
                    }
                }
            }

            throw new Exception("Did not find a path! :(");
        }

        private IEnumerable<Point2D> GetNeighboursFor(Point2D current)
        {
            if (current.X > 0)
            {
                yield return current with { X = current.X - 1 };
            }

            if (current.X < _map.SizeX - 1)
            {
                yield return current with { X = current.X + 1 };
            }
            
            if (current.Y > 0)
            {
                yield return current with { Y = current.Y - 1 };
            }

            if (current.Y < _map.SizeY - 1)
            {
                yield return current with { Y = current.Y + 1 };
            }
        }

        private Traversal RecreatePath()
        {
            var stack = new Stack<Point2D>();
            stack.Push(_endPoint);

            // Note: starting point is not part of `_optimalPreviousPoint`.
            while (_optimalPreviousPoint.TryGetValue(stack.Peek(), out Point2D previousPoint))
            {
                stack.Push(previousPoint);
            }
            return new Traversal(stack, _totalCostPerPoint[_endPoint]);
        }

        /// <remarks>This is the heuristic function for the A* implementation.</remarks>
        private static int EstimateLowestRiskToGetToTarget(Point2D source, Point2D target)
        {
            // Lowest risk for any given square is 1, therefore calculating the 'manhatten distance'
            // also calculated the lowest risk possible to reaching the target:
            return Math.Abs(source.X - target.X) + 
                   Math.Abs(source.Y - target.Y);
        }
    }
}