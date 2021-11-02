using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.MetadataModule.Core.Exceptions
{
    /// <summary>
    /// Incompatible projections exceptions
    /// </summary>
    public class IncompatibleTypesForProjectionOperationException : Exception
    {
        public string FirstProjectionType { get; set; }
        public string SecondProjectionType { get; set; }
        public IncompatibleTypesForProjectionOperationException(string FirstProjectionType, string SecondProjectionType)
        {
            this.FirstProjectionType = FirstProjectionType;
            this.SecondProjectionType = SecondProjectionType;
        }
    }

}
