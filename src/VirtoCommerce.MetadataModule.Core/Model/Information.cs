using System;
using System.Collections;
using System.Linq;
using VirtoCommerce.MetadataModule.Core.Exceptions;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.MetadataModule.Core.Model
{
    public sealed class Information
    {
        private Information() { }

        private static readonly TypePropertyAtrValueCollection cachePropertyType = new TypePropertyAtrValueCollection();
        private static readonly TypePropertyAtrValueCollection cacheGetItemType = new TypePropertyAtrValueCollection();
        private static readonly TypeAtrValueCollection cacheAllPropertyNames = new TypeAtrValueCollection();

        static public Type GetPropertyType(Type declarationType, string propname)
        {
            var result = (Type)cachePropertyType[declarationType, propname];
            if (result != null)
                return result;
            else
            {
                var pointIndex = propname.IndexOf(".");
                if (pointIndex >= 0)
                {
                    var MasterName = propname.Substring(0, pointIndex);
                    var mpropname = propname.Substring(pointIndex + 1);
                    var MasterType = GetPropertyType(declarationType, MasterName);
                    if (typeof(IEnumerable).IsAssignableFrom(MasterType) && MasterType.IsGenericType)
                        MasterType = GetItemType(declarationType, MasterName);
                    if (MasterType == null)
                        throw new CantFindPropertyException(MasterName, declarationType);
                    else
                        result = GetPropertyType(MasterType, mpropname);
                }
                else
                {
                    var pi = declarationType.GetProperty(propname);
                    if (pi == null)
                        throw new CantFindPropertyException(propname, declarationType);
                    /*
                    if (propname == "__PrimaryKey")
                        res = KeyGen.KeyGenerator.Generator(declarationType).KeyType;

                    else
                    {*/
                    Type ptype = pi.PropertyType;
                    result = ptype;
                    /*}*/
                }

                lock (cachePropertyType)
                {
                    if (cachePropertyType[declarationType, propname] == null)
                    {
                        cachePropertyType[declarationType, propname] = result;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Discover the enumerable generic type parameter
        /// </summary>
        /// <param name="AgregatorType">Aggregator model type</param>
        /// <param name="DetailPropertyName">Property name of the aggregator type that points to detail collection</param>
        /// <returns></returns>
        static public Type GetItemType(Type AgregatorType, string DetailPropertyName)
        {

            lock (cacheGetItemType)
            {
                var res = (Type)cacheGetItemType[AgregatorType, DetailPropertyName];
                if (res != null)
                    return res;
                else
                {

                    var pointIndex = DetailPropertyName.IndexOf(".");
                    if (pointIndex >= 0)
                    {
                        var MasterName = DetailPropertyName.Substring(0, pointIndex);
                        DetailPropertyName = DetailPropertyName.Substring(pointIndex + 1);
                        var MasterType = GetPropertyType(AgregatorType, MasterName);
                        if (MasterType == null)
                            throw new CantFindPropertyException(MasterName, AgregatorType);
                        else
                            res = GetItemType(MasterType, DetailPropertyName);
                        return res;
                    }
                    else
                    {
                        var err = string.Empty;

                        try
                        {
                            // TODO: Discover Ienumerable generic type parameter

                            Type propType = GetPropertyType(AgregatorType, DetailPropertyName);
                            if (typeof(IEnumerable).IsAssignableFrom(propType) && propType.IsGenericType)
                            {
                                res = propType.GetGenericArguments().FirstOrDefault();
                            }
                            else
                            {
                                res = null;
                            }
                        }
                        catch
                        {
                            throw new PlatformException("Information getItemType(" + ((AgregatorType == null) ? "NULL" : AgregatorType.FullName) + "," + DetailPropertyName + ")" +
                                Environment.NewLine + err);
                        }

                        err = "9";

                        cacheGetItemType[AgregatorType, DetailPropertyName] = res;

                        return res;
                    }
                }
            }
        }

        static public string[] GetAllPropertyNames(Type type)
        {
            lock (cacheAllPropertyNames)
            {
                var res = (string[])cacheAllPropertyNames[type];
                if (res != null)
                    return CopyStringArray(res);
                else
                {
                    var returnValue = type.GetProperties().Select(x => x.Name).ToArray();
                    cacheAllPropertyNames[type] = returnValue;
                    return returnValue;
                }
            }
        }


        static private string[] CopyStringArray(string[] a)
        {
            if (a == null)
                return null;
            else
                return (string[])a.Clone();
        }
    }
}
