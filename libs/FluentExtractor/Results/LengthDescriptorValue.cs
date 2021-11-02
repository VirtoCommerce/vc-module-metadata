namespace FluentExtractor.Results
{
    public sealed class LengthDescriptorValue : IDescriptorValue
    {
        public int From
        {
            get;
            internal set;
        }

        public int To
        {
            get;
            internal set;
        }

        public LengthDescriptorValue(int from, int to)
        {
            From = from;
            To = to;
        }

        public override bool Equals(object obj)
        {
            if (obj is LengthDescriptorValue lengthDescriptorValue)
            {
                return lengthDescriptorValue.From == From && lengthDescriptorValue.To == To;
            }

            return false;
        }
    }
}
