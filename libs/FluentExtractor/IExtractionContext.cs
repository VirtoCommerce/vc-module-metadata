using FluentExtractor.Internal;

namespace FluentExtractor
{
    public interface IExtractionContext
    {
        PropertyChain PropertyChain { get; }

        IExtractorSelector Selector { get; }
    }
}
