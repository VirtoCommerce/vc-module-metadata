using FluentExtractor.Results;

namespace FluentExtractor.Extractors.Length
{
    public sealed class MaxLengthExtractor<T> : Extractor<T, string>
    {
        public override string MetadataKey => "Length";

        private readonly int to;

        public MaxLengthExtractor(int to) => this.to = to;

        public override void Extract(ExtractionContext<T> context)
        {
            if (TryGetCurrentValue<LengthDescriptorValue>(context, out var lengthDescriptionValue))
            {
                lengthDescriptionValue.To = to;
            }

            SetDefault(context);
        }

        protected override IDescriptorValue DefaultValue => new LengthDescriptorValue(0, to);
    }
}
