namespace VirtoCommerce.MetadataModule.Core.Model
{
    /// <summary>
    /// Master in the projection definition
    /// </summary>
    public class Roots
    {

        public Roots()
        {
        }

        /// <summary>
        /// Master property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// UI control additional customization info 
        /// </summary>
        public string CustomizationString { get; set; }

        /// <summary>
        /// Lookup type (how the lookup control should looking at all)
        /// </summary>
        public LookupType LookupType { get; set; }

        /// <summary>
        /// A valuable property for user to show in the lookup UI-control
        /// </summary>
        public string LookupProperty { get; set; }
    }
}
