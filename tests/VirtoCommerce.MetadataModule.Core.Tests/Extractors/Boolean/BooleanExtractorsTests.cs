using FluentExtractor.Results;
using FluentExtractor.Tests.Helpers;
using FluentExtractor.Tests.Models;
using Xunit;

namespace FluentExtractor.Tests.Extractors.Boolean
{
    public class BooleanExtractorsTests : ExtractorTestsHelper
    {
        protected override string MetadataKey => "Required";
        protected override string PropertyName => nameof(TestModel.TestProperty);

        [Fact]
        public void RequiredDescriptor()
        {
            // Arrange
            var expected = GetExpected(new BooleanDescriptorValue(true));

            // Act
            var metadata = _extractor.Extract(nameof(RequiredDescriptor));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }
    }
}
