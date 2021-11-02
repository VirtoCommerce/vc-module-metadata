namespace FluentExtractor.Internal
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using FluentExtractor;
    using FluentExtractor.Descriptors;

    internal abstract class ConfigBase<T, TValue> : IValidationRule<T, TValue>
    {
        private readonly List<ConfigComponent<T, TValue>> _components = new List<ConfigComponent<T, TValue>>();

        public List<ConfigComponent<T, TValue>> Components => _components;

        public string Projection { get; set; }

        protected ConfigBase(MemberInfo member, LambdaExpression expression)
        {
            var containerType = typeof(T);
            PropertyName = ExtractorOptions.Global.PropertyNameResolver(containerType, member, expression);
        }

        public void SetExtractor(IExtractor<T, TValue> validator)
        {
            var component = new ConfigComponent<T, TValue>(validator);
            _components.Add(component);
        }

        public string PropertyName
        {
            get;
            set;
        }
    }
}
