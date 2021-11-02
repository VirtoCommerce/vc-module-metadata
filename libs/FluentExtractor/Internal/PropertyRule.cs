using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentExtractor.Internal
{
    internal class PropertyRule<T, TProperty> : ConfigBase<T, TProperty>, IExtractionRuleInternal<T, TProperty>
    {
        public PropertyRule(MemberInfo member, LambdaExpression expression, Type typeToValidate)
            : base(member, expression)
        {
        }

        public static PropertyRule<T, TProperty> Create(Expression<Func<T, TProperty>> expression)
        {
            var member = expression.GetMember();

            return new PropertyRule<T, TProperty>(member, expression, typeof(TProperty));
        }

        public virtual void Extract(ExtractionContext<T> context)
        {
            var propertyName = context.PropertyChain.BuildPropertyName(PropertyName);

            if (!context.Selector.CanExecute(this))
                return;

            context.SetPropertyName(propertyName);

            foreach (var step in Components)
            {
                step.Extract(context);
            }
        }
    }
}
