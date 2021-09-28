using System;
using System.Collections.Generic;

namespace VirtoCommerce.MetadataModule.Core.Model
{
    public class TypeAtrValueCollection
    {
        private readonly Dictionary<int, object> sl = new Dictionary<int, object>();

        public TypeAtrValueCollection()
        {
        }

        public int Count
        {
            get { return sl.Count; }
        }

        public object this[Type tp]
        {
            get
            {
                var key = tp.GetHashCode();
                if (sl.TryGetValue(key, out var retObj))
                {
                    return retObj;
                }
                return null;
            }
            set
            {
                var key = tp.GetHashCode();

                if (sl.ContainsKey(key)) sl.Remove(key);
                sl.Add(key, value);
            }
        }
    }


}
