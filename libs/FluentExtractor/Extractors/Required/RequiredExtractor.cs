using FluentExtractor.Results;

namespace FluentExtractor.Extractors.Required
{
    public class RequiredExtractor<T, TProperty> : Extractor<T, TProperty>
    {
        public override string MetadataKey => "Required";

        private readonly bool isRequired;

        public RequiredExtractor(bool isRequired) => this.isRequired = isRequired;

        protected override IDescriptorValue DefaultValue => new BooleanDescriptorValue(isRequired);
    }
}
