using FluentExtractor.Exceptions;
using FluentExtractor.Results;
using FluentExtractor.Tests.Helpers;
using FluentExtractor.Tests.Models;
using Xunit;

namespace FluentExtractor.Tests.Extractors.Scalar
{
    public class LengthExtractorsTests : ExtractorTestsHelper
    {
        protected override string MetadataKey => "Length";
        protected override string PropertyName => nameof(TestModel.TestProperty);

        #region Length tests

        [Fact]
        public void Length_FromLessZero_Exception()
        {
            // Assert
            Assert.Throws<ExtractionException>(() => _extractor.Extract(nameof(Length_FromLessZero_Exception)));
        }

        [Fact]
        public void Length_FromGreaterTo_Exception()
        {
            // Assert
            Assert.Throws<ExtractionException>(() => _extractor.Extract(nameof(Length_FromGreaterTo_Exception)));
        }

        [Fact]
        public void Length_Returns_0_255()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(0, 255));

            // Act
            var metadata = _extractor.Extract(nameof(Length_Returns_0_255));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        [Fact]
        public void Length_LocalOverride_Returns_10_20()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(10, 20));

            // Act
            var metadata = _extractor.Extract(nameof(Length_LocalOverride_Returns_10_20));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        [Fact]
        public void Length_ExtendedOverride_Returns_30_40()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(30, 40));

            // Act
            var metadata = _extendedExtractor.Extract(nameof(Length_ExtendedOverride_Returns_30_40));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        #endregion Length tests

        #region MinLength tests

        [Fact]
        public void MinLength_Exception()
        {
            // Assert
            Assert.Throws<ExtractionException>(() => _extractor.Extract(nameof(MinLength_Exception)));
        }

        [Fact]
        public void MinLength_Returns_60_Max()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(60, int.MaxValue));

            // Act
            var metadata = _extractor.Extract(nameof(MinLength_Returns_60_Max));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        [Fact]
        public void MinLength_LocalOverride_Returns_70_Max()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(70, int.MaxValue));

            // Act
            var metadata = _extractor.Extract(nameof(MinLength_LocalOverride_Returns_70_Max));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        [Fact]
        public void MinLength_ExtendedOverride_Returns_70_Max()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(70, int.MaxValue));

            // Act
            var metadata = _extendedExtractor.Extract(nameof(MinLength_ExtendedOverride_Returns_70_Max));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        #endregion MinLength tests

        #region MaxLength tests

        [Fact]
        public void MaxLength_Returns_0_60()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(0, 60));

            // Act
            var metadata = _extractor.Extract(nameof(MaxLength_Returns_0_60));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        [Fact]
        public void MaxLength_LocalOverride_Returns_0_70()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(0, 70));

            // Act
            var metadata = _extractor.Extract(nameof(MaxLength_LocalOverride_Returns_0_70));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        [Fact]
        public void MaxLength_ExtendedOverride_Returns_0_70()
        {
            // Arrange
            var expected = GetExpected(new LengthDescriptorValue(0, 70));

            // Act
            var metadata = _extendedExtractor.Extract(nameof(MaxLength_ExtendedOverride_Returns_0_70));

            // Assert
            metadata.Descriptors.ShouldContainSingle(expected);
        }

        #endregion MaxLength tests
    }
}
