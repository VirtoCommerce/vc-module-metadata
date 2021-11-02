using System;

namespace FluentExtractor.Results
{
    [Serializable]
    public class MetadataDescriptor
    {
        public MetadataDescriptor(string propertyName, string metadataKey, IDescriptorValue value)
        {
            PropertyName = propertyName;
            MetadataKey = metadataKey;
            Value = value;
        }

        public string PropertyName { get; set; }

        public string MetadataKey { get; set; }

        public IDescriptorValue Value { get; set; }
    }
}
