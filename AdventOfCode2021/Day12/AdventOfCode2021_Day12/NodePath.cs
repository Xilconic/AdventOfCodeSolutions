using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021_Day12
{
    internal class NodePath
    {
        private readonly List<Node> _travelList = new();
        
        public NodePath(Node start, Node firstTraversal)
        {
            _travelList.Add(start);
            _travelList.Add(firstTraversal);

            IsCompleted = IsEndNode(firstTraversal);
        }

        private NodePath(IEnumerable<Node> travelList, Node latestTraversal)
        {
            _travelList.AddRange(travelList);
            _travelList.Add(latestTraversal);
            
            IsCompleted = IsEndNode(latestTraversal);
        }

        public bool IsCompleted { get; private set; }
        public bool IsStuck { get; private set; } = false;

        public IEnumerable<NodePath> Explore()
        {
            if(IsCompleted) return Array.Empty<NodePath>();

            var newPaths = new List<NodePath>();
            Node firstExploredNode = null;
            foreach (Node node in _travelList[^1].ConnectedNodes.Where(n => n.Type != NodeType.Start))
            {
                if (node.Type == NodeType.Small && _travelList.Contains(node))
                {
                    // Do not revit the same small caves!
                    continue;
                }
                
                if (firstExploredNode is null)
                {
                    firstExploredNode = node;
                }
                else
                {
                    newPaths.Add(new NodePath(_travelList, node));
                }
            }

            if (firstExploredNode != null)
            {
                _travelList.Add(firstExploredNode);
                IsCompleted = IsEndNode(firstExploredNode);                
            }
            else
            {
                // No more options available without violating backtracking on small caves!
                IsStuck = true;
            }

            return newPaths;
        }

        private static bool IsEndNode(Node firstTraversal) => firstTraversal.Type == NodeType.End;

        public override string ToString() => string.Join(",", _travelList);
    }
}