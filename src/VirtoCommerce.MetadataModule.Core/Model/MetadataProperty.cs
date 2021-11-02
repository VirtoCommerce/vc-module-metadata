namespace VirtoCommerce.MetadataModule.Core.Model
{
    /// <summary>
    /// Definition of the property in the projection
    /// </summary>
    public class MetadataProperty
    {
        public MetadataProperty()
        {
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        /// <summary>
        /// Property/field name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Caption for the field in that projection
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Property/field visibility
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Order of the property/field in the projection to strictly define the position (UI, as example)
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Some formatting applied to the attribute value
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Form control position/path
        /// </summary>
        public string FormPath { get; set; }
    }
}
