using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentExtractor.Results;

namespace FluentExtractor.Tests.Helpers
{
    using ContainSingleResult = AndWhichConstraint<GenericCollectionAssertions<MetadataDescriptor>, MetadataDescriptor>;

    public static class AssertionExtensions
    {
        public static ContainSingleResult ShouldContainSingle(this List<MetadataDescriptor> descriptors, MetadataDescriptor expected)
            => descriptors.Should().ContainSingle(x => x.Value.Equals(expected.Value) && x.PropertyName == expected.PropertyName && x.MetadataKey == expected.MetadataKey);
    }
}
