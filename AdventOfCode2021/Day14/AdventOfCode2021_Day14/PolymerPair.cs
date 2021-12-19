namespace AdventOfCode2021_Day14
{
    internal class PolymerPair
    {
        private string _insertion;

        public PolymerPair(string pair)
        {
            Code = pair;
        }

        public string Code { get; }

        public void InsertInMiddle(string insertion)
        {
            _insertion = insertion;
        }

        public string GetFullCode() => $"{Code[0]}{GetOptionalInsertionWithEndOfCode()}";
        public string GetOptionalInsertionWithEndOfCode() => $"{_insertion ?? string.Empty}{Code[1]}";
    }
}