using FluentExtractor.Results;
using FluentExtractor.Tests.Models;

namespace FluentExtractor.Tests.Helpers
{
    public abstract class ExtractorTestsHelper
    {
        protected readonly TestExtractor _extractor = new TestExtractor();
        protected readonly TestExtractorExtended _extendedExtractor = new TestExtractorExtended();

        protected abstract string PropertyName { get; }

        protected abstract string MetadataKey { get; }

        protected MetadataDescriptor GetExpected(IDescriptorValue value)
            => new MetadataDescriptor(PropertyName, MetadataKey, value);
    }
}
