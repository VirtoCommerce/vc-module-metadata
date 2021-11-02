using FluentExtractor.Descriptors;
using FluentExtractor.Results;

namespace FluentExtractor.Extractors
{
    public abstract class Extractor<T, TProperty> : IExtractor<T, TProperty>
    {
        public abstract string MetadataKey { get; }
        protected abstract IDescriptorValue DefaultValue { get; }

        public virtual void Extract(ExtractionContext<T> context) => SetDefault(context);

        public MetadataDescriptor FindMetadataDescriptor(ExtractionContext<T> context) => context.FindDescriptor(MetadataKey);

        public bool TryGetCurrentValue<TValue>(ExtractionContext<T> context, out TValue value) where TValue : IDescriptorValue
        {
            if (FindMetadataDescriptor(context)?.Value is TValue tDescriptionValue)
            {
                value = tDescriptionValue;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        protected void SetDefault(ExtractionContext<T> context)
        {
            var currentDescriptor = FindMetadataDescriptor(context);
            if (currentDescriptor is null)
            {
                context.AddDescriptor(new MetadataDescriptor(context.PropertyName, MetadataKey, DefaultValue));
                return;
            }

            currentDescriptor.Value = DefaultValue;
        }
    }
}
