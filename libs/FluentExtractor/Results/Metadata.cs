using System.Collections.Generic;

namespace FluentExtractor.Results
{
    public class Metadata
    {
        private readonly List<MetadataDescriptor> _descriptors;

        public List<MetadataDescriptor> Descriptors => _descriptors;

        public Metadata()
        {
            _descriptors = new List<MetadataDescriptor>();
        }

        internal Metadata(List<MetadataDescriptor> descriptors)
        {
            _descriptors = descriptors;
        }
    }
}
