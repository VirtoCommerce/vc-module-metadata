using System;
using FluentExtractor.Descriptors;

namespace FluentExtractor.Internal
{
    internal class RuleBuilder<T, TProperty> : IRuleBuilderOptions<T, TProperty>, IRuleBuilderInitial<T, TProperty>
    {
        public IExtractionRuleInternal<T, TProperty> Rule { get; }

        public RuleBuilder(IExtractionRuleInternal<T, TProperty> rule) => Rule = rule;

        public IRuleBuilderOptions<T, TProperty> SetExtractor(IExtractor<T, TProperty> extractor)
        {
            if (extractor == null) throw new ArgumentNullException(nameof(extractor));
            Rule.SetExtractor(extractor);
            return this;
        }
    }
}
