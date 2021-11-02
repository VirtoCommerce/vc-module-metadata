using FluentExtractor.Descriptors;

namespace FluentExtractor.Internal
{
    public class ConfigComponent<T, TProperty>
    {
        private readonly IExtractor<T, TProperty> _propertyExtractor;

        internal ConfigComponent(IExtractor<T, TProperty> propertyExtractor) => _propertyExtractor = propertyExtractor;

        internal virtual void Extract(ExtractionContext<T> context) => _propertyExtractor.Extract(context);
    }
}
