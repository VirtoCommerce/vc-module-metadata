namespace FluentExtractor
{
    using System;
    using FluentExtractor.Internal;
    using FluentExtractor.Results;

    public interface IExtractor<T>
    {
        Metadata Extract(Action<ExtractorPicker<T>> options);
    }
}
