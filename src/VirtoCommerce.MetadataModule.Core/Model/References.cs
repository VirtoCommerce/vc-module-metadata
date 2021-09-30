using System;

namespace VirtoCommerce.MetadataModule.Core.Model
{

    /// <summary>
    /// Detail projection in the aggregator projection
    /// </summary>
    [Serializable]
    public class References
    {

        public References()
        {

        }

        /// <summary>
        /// Projection, defined for detail model
        /// </summary>
        public Projection Projection { get; set; }

        /// <summary>
        /// Name of the property which the projection defined for
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property caption
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Property form path
        /// </summary>
        public string FormPath { get; set; }

        /// <summary>
        /// Order of the property
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Should the detail objects be loaded together with aggregator or not
        /// </summary>
        public bool LoadOnLoadAgregator { get; set; }

        /// <summary>
        /// Define the property visible or not
        /// </summary>
        public bool Visible { get; set; }

    }
}
