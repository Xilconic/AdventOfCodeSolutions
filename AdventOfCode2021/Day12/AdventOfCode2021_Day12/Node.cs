using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2021_Day12
{
    internal class Node
    {
        public Node(string identifier)
        {
            Debug.Assert(identifier.Length > 0);
            Identifier = identifier;
            Type = DetermineType(identifier);
        }
        
        public string Identifier { get; }
        public NodeType Type { get; }

        public ICollection<Node> ConnectedNodes { get; } = new List<Node>();

        private NodeType DetermineType(string identifier)
        {
            switch (identifier)
            {
                case "start": return NodeType.Start;
                case "end": return NodeType.End;
                default: return IsCapitolLetter(identifier[0]) ? NodeType.Large : NodeType.Small;
            }
        }
        
        /// <remarks><see cref="char"/> is UTF-16 value. The range [65-90] defines capitol letters.</remarks>
        private static bool IsCapitolLetter(char character) => 65 <= character && character <= 90;

        public override string ToString() => Identifier;
    }
}