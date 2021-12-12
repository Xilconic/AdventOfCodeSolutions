using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode2021_Day12
{
    internal class Cave
    {
        private const string StartNodeName = "start";
        private const string EndNodeName = "end";

        private readonly Node _end  = new(EndNodeName);
        private readonly Dictionary<string, Node> _caverns = new();

        public void Connect(string node1Name, string node2Name)
        {
            Node node1 = GetNode(node1Name);
            Node node2 = GetNode(node2Name);
            
            node1.ConnectedNodes.Add(node2);
            node2.ConnectedNodes.Add(node1);
        }
        
        public Node Start { get; } = new(StartNodeName);

        private Node GetNode(string name)
        {
            switch (name)
            {
                case StartNodeName: return Start;
                case EndNodeName: return _end;
                default:
                {
                    if (_caverns.TryGetValue(name, out Node existingCavernNode))
                    {
                        return existingCavernNode;
                    }

                    var newNode = new Node(name);
                    _caverns.Add(name, newNode);

                    return newNode;
                }
            }
        }

        public void PruneUntraversableDeadEnds()
        {
            var deadEndSmallNodes = _caverns.Values
                .Where(IsPrunableDeadEnd)
                .ToArray();
            if (deadEndSmallNodes.Length == 0) return;
            
            foreach (Node nodeToBePruned in deadEndSmallNodes)
            {
                Prune(nodeToBePruned);
            }
                
            PruneUntraversableDeadEnds();
        }

        private static bool IsPrunableDeadEnd(Node n)
        {
            switch (n.Type)
            {
                case NodeType.Small:
                case NodeType.Large:
                    return n.ConnectedNodes.Count == 1 && n.ConnectedNodes.First().Type == NodeType.Small;
                default: return false;
            }
        }

        private void Prune(Node nodeToBePruned)
        {
            Start.ConnectedNodes.Remove(nodeToBePruned);
            _end.ConnectedNodes.Remove(nodeToBePruned);
            foreach (Node node in _caverns.Values)
            {
                node.ConnectedNodes.Remove(nodeToBePruned);
            }

            _caverns.Remove(nodeToBePruned.Identifier);
        }
    }
}