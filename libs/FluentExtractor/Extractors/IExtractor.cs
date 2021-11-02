using FluentExtractor.Results;

namespace FluentExtractor.Descriptors
{
    public interface IExtractor<T, in TProperty> : IPropertyMetadataExtractor
    {
        void Extract(ExtractionContext<T> context);
    }

    // TODO: Need refactor
    public interface IPropertyMetadataExtractor
    {
        string MetadataKey { get; }
    }
}
