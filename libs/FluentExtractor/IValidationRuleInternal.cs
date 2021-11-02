namespace FluentExtractor
{
    using System.Collections.Generic;
    using FluentExtractor.Internal;

    internal interface IExtractionRuleInternal<T> : IExtractionRule
    {
        void Extract(ExtractionContext<T> context);
    }

    internal interface IExtractionRuleInternal<T, TProperty> : IValidationRule<T, TProperty>, IExtractionRuleInternal<T>
    {
        new List<ConfigComponent<T, TProperty>> Components { get; }
    }
}
