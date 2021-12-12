using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021_Day12
{
    internal class CavePathFinder
    {
        private readonly Cave _cave;

        private CavePathFinder(Cave cave)
        {
            _cave = cave;
        }

        public static CavePathFinder FromFile(string filename)
        {
            var cave = new Cave();
            foreach (string line in File.ReadLines(filename))
            {
                var match = Regex.Match(line, @"(?<n1>\w+)-(?<n2>\w+)");
                string node1Name = match.Groups["n1"].Value;
                string node2Name = match.Groups["n2"].Value;
                cave.Connect(node1Name, node2Name);
            }

            return new CavePathFinder(cave);
        }

        public void PruneUntraversableDeadEnds()
        {
            // Note: Leaf-node small caves can be pruned from the graph, as those cannot ever be part of a valid path.
            // Note: Leaf-Node Large case can be pruned from the graph if those are connected to a small cave, as we're not allowed to backtrack then.
            _cave.PruneUntraversableDeadEnds();
        }

        public IReadOnlyCollection<NodePath> GetAllPathsVisitingSmallCavesAtMostOnce()
        {
            // Note: In order to prevent infinite cycles, graph cannot have 2 big caves connecting to each other.
            var paths = _cave.Start.ConnectedNodes
                .Select(n => new NodePath(_cave.Start, n))
                .ToHashSet(); // Made hashset, to speed up removal of elements from the list

            var completedPaths = paths.Where(p => p.IsCompleted).ToList();
            foreach (NodePath completedPath in completedPaths)
            {
                paths.Remove(completedPath);
            }
            while (paths.Count > 0)
            {
                var newPaths = new List<NodePath>();
                foreach (NodePath path in paths)
                {
                    newPaths.AddRange(path.Explore());
                }

                foreach (NodePath newPath in newPaths)
                {
                    paths.Add(newPath);
                }

                var newlyCompletedPaths = new List<NodePath>();
                var stuckPaths = new List<NodePath>();
                foreach (NodePath path in paths)
                {
                    if (path.IsCompleted)
                    {
                        newlyCompletedPaths.Add(path);
                    }
                    else if (path.IsStuck)
                    {
                        stuckPaths.Add(path);
                    }
                }
                foreach (NodePath completedPath in newlyCompletedPaths.Concat(stuckPaths))
                {
                    paths.Remove(completedPath);
                }
                completedPaths.AddRange(newlyCompletedPaths);
            }
            
            return completedPaths;
        }

        public IReadOnlyCollection<NodePath> GetAllPathsVisitingOnlyOneSmallCaveAtMostTwice()
        {
            // Note: In order to prevent infinite cycles, graph cannot have 2 big caves connecting to each other.
            var paths = _cave.Start.ConnectedNodes
                .Select(n => new NodePath(_cave.Start, n))
                .ToHashSet(); // Made hashset, to speed up removal of elements from the list

            var completedPaths = paths.Where(p => p.IsCompleted).ToList();
            foreach (NodePath completedPath in completedPaths)
            {
                paths.Remove(completedPath);
            }
            while (paths.Count > 0)
            {
                var newPaths = new List<NodePath>();
                foreach (NodePath path in paths)
                {
                    newPaths.AddRange(path.SlightlyScenicExplore());
                }

                foreach (NodePath newPath in newPaths)
                {
                    paths.Add(newPath);
                }

                var newlyCompletedPaths = new List<NodePath>();
                var stuckPaths = new List<NodePath>();
                foreach (NodePath path in paths)
                {
                    if (path.IsCompleted)
                    {
                        newlyCompletedPaths.Add(path);
                    }
                    else if (path.IsStuck)
                    {
                        stuckPaths.Add(path);
                    }
                }
                foreach (NodePath completedPath in newlyCompletedPaths.Concat(stuckPaths))
                {
                    paths.Remove(completedPath);
                }
                completedPaths.AddRange(newlyCompletedPaths);
            }
            
            return completedPaths;
        }
    }
}