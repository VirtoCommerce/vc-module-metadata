using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.MetadataModule.Core.Model;

namespace VirtoCommerce.MetadataModule.Core.Services
{
    /// <summary>
    /// An interface to a service allowing to define/consume model projections
    /// </summary>
    public interface IMetadataService
    {
        /// <summary>
        /// Add projection to the projections list
        /// </summary>
        /// <param name="projection"></param>
        public void AddProjection(Projection projection);

        /// <summary>
        /// Get the projection for specific model type and projection name
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="projectionName"></param>
        /// <returns></returns>
        public Projection GetProjection(Type modelType, string projectionName);

        /// <summary>
        /// Get all registered model types
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Type> GetModels();

        /// <summary>
        /// Get all registered projections for specific model type
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public IReadOnlyCollection<Projection> GetProjections(Type modelType);
    }
}
