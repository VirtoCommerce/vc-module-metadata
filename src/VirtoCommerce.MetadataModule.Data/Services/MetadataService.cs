using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VirtoCommerce.MetadataModule.Core.Model;
using VirtoCommerce.MetadataModule.Core.Services;

namespace VirtoCommerce.MetadataModule.Data.Services
{
    public class MetadataService : IMetadataService
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Projection>> _projections = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Projection>>();

        public virtual void AddProjection(Projection projection)
        {
            var classProjections = _projections.GetOrAdd(projection.DefineClassType, new ConcurrentDictionary<string, Projection>());
            classProjections.GetOrAdd(projection.Name, projection);
        }

        public virtual Projection GetProjection(Type modelType, string projectionName)
        {
            return _projections[modelType][projectionName];
        }

        public virtual IReadOnlyCollection<Type> GetModels()
        {
            return new ReadOnlyCollection<Type>(_projections.Keys.ToList());
        }

        public virtual IReadOnlyCollection<Projection> GetProjections(Type modelType)
        {
            return new ReadOnlyCollection<Projection>(_projections[modelType].Values.ToList());
        }
    }
}
