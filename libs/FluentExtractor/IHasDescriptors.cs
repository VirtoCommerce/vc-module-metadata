using System.Collections.Generic;
using FluentExtractor.Results;

namespace FluentExtractor
{
    internal interface IHasDescriptors
    {
        List<MetadataDescriptor> Descriptors { get; }
    }
}
