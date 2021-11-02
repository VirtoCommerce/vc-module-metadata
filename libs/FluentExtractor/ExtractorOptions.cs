using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentExtractor.Internal;

namespace FluentExtractor
{
    /// <summary>
    /// Configuration options for validators.
    /// </summary>
    public class ValidatorConfiguration
    {
        private Func<Type, MemberInfo, LambdaExpression, string> _propertyNameResolver = DefaultPropertyNameResolver;

        public string PropertyChainSeparator { get; set; } = ".";

        public ValidatorSelectorOptions ExtractorSelectors { get; } = new ValidatorSelectorOptions();

        public Func<Type, MemberInfo, LambdaExpression, string> PropertyNameResolver
        {
            get => _propertyNameResolver;
            set => _propertyNameResolver = value ?? DefaultPropertyNameResolver;
        }

        private static string DefaultPropertyNameResolver(Type type, MemberInfo memberInfo, LambdaExpression expression)
        {
            if (expression != null)
            {
                var chain = PropertyChain.FromExpression(expression);
                if (chain.Count > 0) return chain.ToString();
            }

            return memberInfo?.Name;
        }
    }

    public static class ExtractorOptions
    {
        public static ValidatorConfiguration Global { get; } = new ValidatorConfiguration();
    }

    public class ValidatorSelectorOptions
    {
        private static readonly IExtractorSelector DefaultSelector = new DefaultExtractorSelector();

        private Func<IExtractorSelector> _defaultValidatorSelector = () => DefaultSelector;

        private Func<string, IExtractorSelector> _rulesetValidatorSelector = ruleSet => new ProjectionSelector(ruleSet);

        public Func<IExtractorSelector> DefaultExtractorSelectorFactory
        {
            get => _defaultValidatorSelector;
            set => _defaultValidatorSelector = value ?? (() => new DefaultExtractorSelector());
        }

        public Func<string, IExtractorSelector> ProjectionExtractorSelectorFactory
        {
            get => _rulesetValidatorSelector;
            set => _rulesetValidatorSelector = value ?? (ruleSets => new ProjectionSelector(ruleSets));
        }
    }
}
