using FluentExtractor.Exceptions;
using FluentExtractor.Results;

namespace FluentExtractor.Extractors.Length
{
    public sealed class LengthExtractor<T> : Extractor<T, string>
    {
        public override string MetadataKey => "Length";

        private readonly int from;
        private readonly int to;

        public LengthExtractor(int from, int to)
        {
            this.from = from;
            this.to = to;
        }

        public override void Extract(ExtractionContext<T> context)
        {
            if (from < 0) throw new ExtractionException($"{nameof(from)} should be greater than zero");

            if (from > to) throw new ExtractionException($"{nameof(from)} should be less than {nameof(to)}");

            if (TryGetCurrentValue<LengthDescriptorValue>(context, out var lengthDescriptionValue))
            {
                lengthDescriptionValue.To = to;
                lengthDescriptionValue.From = from;
            }

            SetDefault(context);
        }

        protected override IDescriptorValue DefaultValue => new LengthDescriptorValue(from, to);
    }
}
