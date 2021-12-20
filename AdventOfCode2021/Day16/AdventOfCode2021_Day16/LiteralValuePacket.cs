namespace AdventOfCode2021_Day16
{
    internal class LiteralValuePacket : Packet
    {
        public LiteralValuePacket(int version, int typeId, long literalValue)
            : base(version, typeId)
        {
            Value = literalValue;
        }

        public override long Value { get; }
    }
}