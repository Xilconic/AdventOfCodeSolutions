namespace AdventOfCode2021_Day16
{
    internal abstract class Packet
    {
        protected Packet(
            int versionNumber,
            int typeId)
        {
            VersionNumber = versionNumber;
            TypeId = typeId;
        }

        public int VersionNumber { get; }
        public int TypeId { get; }
        public abstract long Value { get; }
    }
}