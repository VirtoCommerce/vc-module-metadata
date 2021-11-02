using System;
using System.Linq.Expressions;
using FluentExtractor.Internal;
using FluentExtractor.Results;

namespace FluentExtractor
{
    public abstract class AbstractExtractor<T> : IExtractor<T>
    {
        internal TrackingList<IExtractionRuleInternal<T>> Rules { get; } = new TrackingList<IExtractionRuleInternal<T>>();

        public virtual Metadata Extract(Action<ExtractorPicker<T>> options)
        {
            options.Guard("Cannot pass null to Extract", nameof(options));
            var context = ExtractionContext<T>.CreateWithOptions(options);

            var result = new Metadata(context.Descriptors);

            foreach (var rule in Rules)
            {
                rule.Extract(context);
            }

            return result;
        }

        protected IRuleBuilderInitial<T, TProperty> Configure<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            expression.Guard("Cannot pass null to Configure", nameof(expression));
            var rule = PropertyRule<T, TProperty>.Create(expression);
            Rules.Add(rule);
            return new RuleBuilder<T, TProperty>(rule);
        }

        protected void Projection(string projectionName, Action action)
        {
            projectionName.Guard("A name must be specified when calling Projection.", nameof(projectionName));
            action.Guard("A configure definition must be specified when calling Projection.", nameof(action));

            using (Rules.OnItemAdded(r => r.Projection = projectionName))
            {
                action();
            }
        }
    }
}
