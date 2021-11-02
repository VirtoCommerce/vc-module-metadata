using FluentExtractor.Exceptions;
using FluentExtractor.Results;

namespace FluentExtractor.Extractors.Length
{
    public sealed class MinLengthExtractor<T> : Extractor<T, string>
    {
        public override string MetadataKey => "Length";

        private readonly int from;

        public MinLengthExtractor(int from) => this.from = from;

        public override void Extract(ExtractionContext<T> context)
        {
            if (from < 0) throw new ExtractionException($"{nameof(from)} should be greater than zero");

            if (TryGetCurrentValue<LengthDescriptorValue>(context, out var lengthDescriptionValue))
            {
                lengthDescriptionValue.From = from;
            }

            SetDefault(context);
        }

        protected override IDescriptorValue DefaultValue => new LengthDescriptorValue(from, int.MaxValue);
    }
}
