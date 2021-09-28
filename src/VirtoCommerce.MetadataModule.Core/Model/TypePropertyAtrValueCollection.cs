using System;
using System.Collections.Generic;

namespace VirtoCommerce.MetadataModule.Core.Model
{
    public class TypePropertyAtrValueCollection
    {
        private readonly Dictionary<long, object> sl = new Dictionary<long, object>();

        public TypePropertyAtrValueCollection()
        {
        }

        public int Count
        {
            get { return sl.Count; }
        }

        public object this[Type tp, string propname]
        {
            get
            {

                var key = tp.GetHashCode() * 10000000000 + propname.GetHashCode();
                if (sl.TryGetValue(key, out var retObj))
                {
                    return retObj;
                }
                return null;
            }
            set
            {
                var key = tp.GetHashCode() * 10000000000 + propname.GetHashCode();

                if (sl.ContainsKey(key)) sl.Remove(key);
                sl.Add(key, value);
            }
        }
    }

}
