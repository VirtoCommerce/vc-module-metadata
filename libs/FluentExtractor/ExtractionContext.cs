using System;
using System.Collections.Generic;
using FluentExtractor.Internal;
using FluentExtractor.Results;

namespace FluentExtractor
{
    public class ExtractionContext<T> : IExtractionContext, IHasDescriptors
    {
        List<MetadataDescriptor> IHasDescriptors.Descriptors => Descriptors;
        internal List<MetadataDescriptor> Descriptors { get; }

        internal MetadataDescriptor FindDescriptor(string metadataKey) => Descriptors.Find(x => x.MetadataKey == metadataKey);

        internal ExtractionContext(IExtractorSelector validatorSelector)
        {
            PropertyChain propertyChain = null;
            PropertyChain = new PropertyChain(propertyChain);
            Selector = validatorSelector;
            Descriptors = new List<MetadataDescriptor>();
        }

        public static ExtractionContext<T> CreateWithOptions(Action<ExtractorPicker<T>> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            var strategy = new ExtractorPicker<T>();
            options(strategy);
            return strategy.BuildContext();
        }

        public PropertyChain PropertyChain { get; private set; }

        public IExtractorSelector Selector { get; private set; }

        public void AddDescriptor(MetadataDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor), "A descriptor must be specified when calling AddDescriptor");

            Descriptors.Add(descriptor);
        }

        public string PropertyName { get; private set; }

        internal void SetPropertyName(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
