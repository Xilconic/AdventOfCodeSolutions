using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021_Day16
{
    internal class OperatorPacket : Packet
    {
        public OperatorPacket(int version, int typeId, IReadOnlyCollection<Packet> packages)
            : base(version, typeId)
        {
            SubPackets = packages;
            switch (typeId)
            {
                case 0:
                    Value = SubPackets.Sum(p => p.Value);
                    break;
                case 1:
                    Value = SubPackets.Aggregate(1L, (aggregate, packet) => aggregate * packet.Value);
                    break;
                case 2:
                    Value = SubPackets.Min(p => p.Value);
                    break;
                case 3:
                    Value = SubPackets.Max(p => p.Value);
                    break;
                case 5:
                    Value = SubPackets.First().Value > SubPackets.Last().Value ? 1 : 0;
                    break;
                case 6:
                    Value = SubPackets.First().Value < SubPackets.Last().Value ? 1 : 0;
                    break;
                case 7:
                    Value = SubPackets.First().Value == SubPackets.Last().Value ? 1 : 0;
                    break;
            }
        }

        public IReadOnlyCollection<Packet> SubPackets { get; }
        public override long Value { get; }
    }
}