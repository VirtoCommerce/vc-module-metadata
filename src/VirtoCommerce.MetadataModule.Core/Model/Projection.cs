using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using VirtoCommerce.MetadataModule.Core.Exceptions;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.MetadataModule.Core.Model
{
    /// <summary>
    /// Projection definition
    /// </summary>
    public sealed class Projection : ICloneable
    {
        /// <summary>
        /// The model class for which the projection defined for
        /// </summary>
        [JsonIgnore]
        public Type DefineClassType { get; set; }

        /// <summary>
        /// Name of the projection
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// How the projection was created (notice)
        /// </summary>
        [JsonIgnore]
        public ReadType ReadType { get; private set; }

        /// <summary>
        /// Properties or fields of the model class, represented as the single value
        /// </summary>
        public PropertyInProjection[] Properties { get; set; } = new PropertyInProjection[0];

        /// <summary>
        /// Details (instances, referenced by composition at the side with cardinality *)
        /// </summary>
        public DetailInProjection[] Details { get; set; } = new DetailInProjection[0];

        /// <summary>
        /// Masters (instances, referenced by association at the side with cardinality 0..1 or 1)
        /// </summary>
        public MasterInProjection[] Masters { get; set; } = new MasterInProjection[0];

        public object Clone()
        {
            var res = (Projection)MemberwiseClone();
            res.Properties = new PropertyInProjection[Properties.Length];
            Properties.CopyTo(res.Properties, 0);
            res.Masters = new MasterInProjection[Masters.Length];
            Masters.CopyTo(res.Masters, 0);
            res.Details = new DetailInProjection[Details.Length];
            Details.CopyTo(res.Details, 0);

            return res;
        }

        public Projection()
        {
        }

        public Projection(Type modelType, IEnumerable<string> props)
        {
            DefineClassType = modelType;
            AddProperties(props.ToArray());
        }


        /// <summary>
        /// Create default projection (include all the fields).
        /// Useful for samples, etc... Big projections commonly is a bad practice, because so many data to store/transmit.
        /// </summary>
        /// <param name="ModelType">Type of the model class</param>
        /// <param name="readType">Default projection creation rule</param>
        public Projection(Type ModelType, ReadType readType)
        {
            ReadType = readType;

            Name = ModelType.FullName + "(" + readType.ToString() + ")";
            DefineClassType = ModelType;
            var allstorProps = Information.GetAllPropertyNames(DefineClassType);

            var simpleprops = new StringCollection();
            var detailprops = new StringCollection();

            foreach (var prop in allstorProps)
            {
                var proptype = Information.GetPropertyType(ModelType, prop);
                if (typeof(IEnumerable).IsAssignableFrom(proptype) && proptype.IsGenericType)
                    detailprops.Add(prop);
                else
                {
                    simpleprops.Add(prop);
                }

            }

            Properties = new PropertyInProjection[simpleprops.Count];
            for (var i = 0; i < simpleprops.Count; i++)
            {
                Properties[i] = new PropertyInProjection()
                {
                    Caption = simpleprops[i],
                    Name = simpleprops[i]
                };
            }

            if (readType == ReadType.RelatedDetails)
            {
                Details = new DetailInProjection[detailprops.Count];
                for (var i = 0; i < detailprops.Count; i++)
                {
                    Details[i] = new DetailInProjection() { Name = detailprops[i], Projection = new Projection(Information.GetItemType(DefineClassType, detailprops[i]), ReadType.OnlyThat), Visible = true };
                }
            }

        }

        /// <summary>
        /// Return master-related projection branch
        /// </summary>
        /// <param name="master">Master field name</param>
        /// <returns></returns>
        public Projection GetProjectionForMaster(string master)
        {
            var res = new Projection
            {
                DefineClassType = Information.GetPropertyType(this.DefineClassType, master)
            };
            foreach (var p in Properties)
            {
                if (p.Name.StartsWith(master + "."))
                    res.AddProperty(p.Name.Substring(master.Length + 1), p.Caption, p.Visible, p.FormPath);
            }
            return res;
        }

        /// <summary>
        /// Get specific master in projection description
        /// </summary>
        /// <param name="masterName"></param>
        /// <returns></returns>
        public MasterInProjection GetMaster(string masterName)
        {
            if (Masters == null)
                Masters = new MasterInProjection[0];

            for (var i = 0; i < Masters.Length; i++)
                if (Masters[i].Name == masterName)
                    return Masters[i];
            for (var i = 0; i < Properties.Length; i++)
                if (Properties[i].Name.StartsWith(masterName + "."))
                    return new MasterInProjection() { Name = masterName, LookupType = LookupType.Standard };

            throw new ArgumentOutOfRangeException(masterName);
        }

        /// <summary>
        /// Remove master from the projection
        /// </summary>
        /// <param name="masterName"></param>
        public void RemoveMaster(string masterName)
        {
            if (Masters == null)
                Masters = new MasterInProjection[0];
            if (Masters.Length == 0) return;

            Masters = Masters.SkipWhile(x => x.Name == masterName).ToArray();
        }

        /// <summary>
        /// Return specific detail in projection name
        /// </summary>
        /// <param name="detailName"></param>
        /// <returns></returns>
        public DetailInProjection GetDetail(string detailName)
        {
            if (Details == null)
                Details = new DetailInProjection[0];
            if (Details.Length == 0)
                throw new ArgumentOutOfRangeException(detailName);

            var result = Details.FirstOrDefault(x => x.Name == detailName);
            if (result == null)
            {
                throw new ArgumentOutOfRangeException(detailName, "No such detail property/field found.");
            }

            return result;
        }

        /// <summary>
        /// Remove detail from projection
        /// </summary>
        /// <param name="detailname"></param>
        public void RemoveDetail(string detailname)
        {
            if (Details == null)
                Details = new DetailInProjection[0];
            if (Details.Length == 0) return;

            Details = Details.SkipWhile(x => x.Name == detailname).ToArray();
        }

        /// <summary>
        /// Get specific property in projection description
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public PropertyInProjection GetProperty(string propertyName)
        {
            if (Properties == null)
                Properties = new PropertyInProjection[0];
            if (Properties.Length == 0)
                throw new ArgumentOutOfRangeException(propertyName);

            var result = Properties.FirstOrDefault(x => x.Name == propertyName);

            if (result == null)
            {
                throw new ArgumentOutOfRangeException(propertyName, "No such property/field found.");
            }

            return result;
        }

        /// <summary>
        /// Check if the property exists in projection
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool CheckPropname(string propName)
        {
            return CheckPropname(propName, false);
        }

        /// <summary>
        /// Check if the property exists in projection
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="checkDetails"></param>
        /// <returns></returns>
        public bool CheckPropname(string propName, bool checkDetails)
        {
            return Properties.Any(x => x.Name == propName)
                || (checkDetails && Details.Any(x => x.Name == propName));
        }

        /// <summary>
        /// Remove property from the projection
        /// </summary>
        /// <param name="propName"></param>
        public void RemoveProperty(string propName)
        {
            if (Properties == null)
                Properties = new PropertyInProjection[0];
            if (Properties.Length == 0) return;

            Properties = Properties.SkipWhile(x => x.Name == propName).ToArray();
        }

        /// <summary>
        /// Add detail to the projection
        /// </summary>
        /// <param name="detailname"></param>
        /// <param name="detailProjection"></param>
        /// <param name="loadOnLoadAgregator"></param>
        public void AddDetailInProjection(string detailname, Projection detailProjection, bool loadOnLoadAgregator)
        {
            AddDetailInProjection(detailname, detailProjection, loadOnLoadAgregator, string.Empty, false, string.Empty, null);
        }

        /// <summary>
        /// Add detail to the projection
        /// </summary>
        /// <param name="detailname"></param>
        /// <param name="detailProjection"></param>
        /// <param name="loadOnLoadAgregator"></param>
        /// <param name="path"></param>
        /// <param name="visible"></param>
        /// <param name="caption"></param>
        /// <param name="aggregateFunctions"></param>
        public void AddDetailInProjection(string detailname, Projection detailProjection, bool loadOnLoadAgregator, string path, bool visible, string caption, string[] aggregateFunctions)
        {
            if (Details == null)
                Details = new DetailInProjection[0];
            for (var i = 0; i < Details.Length; i++)
                if (Details[i].Name == detailname)
                    return;
            var divs = Details;
            Details = new DetailInProjection[divs.Length + 1];
            divs.CopyTo(Details, 0);
            Details[divs.Length] = new DetailInProjection() { Name = detailname, Projection = detailProjection, LoadOnLoadAgregator = loadOnLoadAgregator, FormPath = path, Caption = caption, Visible = visible };

        }

        /// <summary>
        /// Add master to the projection
        /// </summary>
        /// <param name="masterName"></param>
        /// <param name="lookupType"></param>
        /// <param name="lookupcustomizationstring"></param>
        /// <param name="lookupProperty"></param>
        public void AddMasterInProjection(string masterName, LookupType lookupType, string lookupcustomizationstring, string lookupProperty)
        {
            if (Masters == null)
                Masters = new MasterInProjection[0];
            for (var i = 0; i < Masters.Length; i++)
                if (Masters[i].Name == masterName)
                    return;
            var oldmasters = Masters;
            Masters = new MasterInProjection[oldmasters.Length + 1];
            oldmasters.CopyTo(Masters, 0);

            Masters[oldmasters.Length] = new MasterInProjection() { Name = masterName, LookupType = lookupType, CustomizationString = lookupcustomizationstring, LookupProperty = lookupProperty };
            if (lookupProperty != string.Empty && !CheckPropname(masterName + "." + lookupProperty))
            {
                AddProperty(masterName + "." + lookupProperty);
            }

        }

        /// <summary>
        /// Add master to the projection
        /// </summary>
        /// <param name="masterName"></param>
        public void AddMasterInProjection(string masterName)
        {
            AddMasterInProjection(masterName, LookupType.Standard, string.Empty, string.Empty);
        }

        /// <summary>
        /// Add properties to the projection
        /// </summary>
        /// <param name="propertyNames"></param>
        public void AddProperties(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                AddProperty(propertyName);
            }
        }

        /// <summary>
        /// Add property to the projection
        /// </summary>
        /// <param name="propName"></param>
        public void AddProperty(string propName)
        {
            AddProperty(propName, string.Empty, false, string.Empty);
        }

        /// <summary>
        /// Add property to the projection
        /// </summary>
        public void AddProperty(string propName, string propCaption, bool visible, string propPath)
        {
            AddProperty(propName, propCaption, visible, propPath, 0, string.Empty);
        }

        /// <summary>
        /// Add property to the projection
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propCaption"></param>
        /// <param name="visible"></param>
        /// <param name="propPath"></param>
        /// <param name="visible"></param>
        /// <param name="propPath"></param>
        public void AddProperty(string propName, string propCaption, bool visible, string propPath, int order, string format)
        {
            lock (this)
            {

                // Check if exists
                if (Properties == null)
                    Properties = new PropertyInProjection[0];
                for (var i = 0; i < Properties.Length; i++)
                    if (Properties[i].Name == propName)
                        return;
                // Increase the count
                var piv = Properties;
                Properties = new PropertyInProjection[piv.Length + 1];
                piv.CopyTo(Properties, 0);
                var propIndex = piv.Length;

                if (propCaption == string.Empty && !propName.EndsWith("*"))
                {
                    propCaption = propName;
                }
                // Add properties by the wildcard *
                if (propName.EndsWith("*"))
                {
                    if (propCaption.EndsWith("*"))
                        propCaption = propCaption.Substring(0, propCaption.Length - 1);

                    var pref = string.Empty;
                    if (propName.LastIndexOf(".") >= 0)
                        pref = propName.Substring(0, propName.LastIndexOf("."));
                    var cType = DefineClassType;
                    var path = pref.Split('.');
                    if (pref != string.Empty)
                    {
                        for (var pathind = 0; pathind < path.Length; pathind++)
                            cType = Information.GetPropertyType(cType, path[pathind]);
                        pref += ".";
                    }
                    var allprops = Information.GetAllPropertyNames(cType);
                    for (var propsind = 0; propsind < allprops.Length; propsind++)
                    {
                        var propType = Information.GetPropertyType(cType, allprops[propsind]);
                        if (!typeof(IEnumerable).IsAssignableFrom(propType) && propType.IsGenericType)
                        {
                            if (propsind != 0)
                            {
                                piv = Properties;
                                Properties = new PropertyInProjection[piv.Length + 1];
                                piv.CopyTo(Properties, 0);
                            }
                            if (propCaption == string.Empty && pref == string.Empty)
                                Properties[propIndex++] = new PropertyInProjection()
                                {
                                    Name = pref + allprops[propsind],
                                    Caption = propCaption + allprops[propsind],
                                    Visible = visible,
                                    FormPath = propPath,
                                    Order = order,
                                    Format = format
                                };
                        }
                    }
                }
                else
                {
                    Properties[propIndex++] = new PropertyInProjection()
                    {
                        Name = propName,
                        Caption = propCaption,
                        Visible = visible,
                        FormPath = propPath,
                        Order = order,
                        Format = format
                    };
                }
            }
        }

        public string AssemblyQualifiedTypeName
        {
            get { return DefineClassType?.AssemblyQualifiedName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    DefineClassType = null;
                else
                {

                    DefineClassType = Type.GetType(value, true);
                }
            }
        }

        /// <summary>
        /// Human-readable view of the projection.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name}({DefineClassType.Name})p{{{string.Join(',', Properties.Select(x => x.Name).ToArray())}}}";
        }

        /// <summary>
        /// Represent full projection 
        /// </summary>
        /// <param name="fullProjection"></param>
        /// <returns></returns>
        public string ToString(bool fullProjection)
        {
            if (!fullProjection)
                return ToString();

            var res = new StringBuilder();
            res.Append(ToString());
            res.Append(":");
            foreach (var pi in Properties)
            {
                res.Append("(");
                res.Append(pi.Name);
                res.Append(",");
                res.Append(pi.Caption);
                res.Append(",");
                res.Append(pi.Visible);
                res.Append(")");
            }

            foreach (var div in Details)
            {
                res.Append("{");
                res.Append(div.Name);
                res.Append(",");
                res.Append(div.Projection);
                res.Append(",");
                res.Append(div.LoadOnLoadAgregator);
                res.Append(")");
            }

            return res.ToString();
        }

        private static Projection getProjectionForProjections(Projection firstProjection, Projection secondProjection)
        {
            var firstType = firstProjection.DefineClassType;
            var secondType = secondProjection.DefineClassType;
            var Result = new Projection();
            if (secondType == firstType)
                Result.DefineClassType = firstType;
            else if (secondType.IsSubclassOf(firstType))
                Result.DefineClassType = secondType;
            else if (firstType.IsSubclassOf(secondType))
                Result.DefineClassType = firstType;
            else
                throw new IncompatibleTypesForProjectionOperationException(firstProjection.ToString(), secondProjection.ToString());
            return Result;
        }

        public static Projection operator |(Projection firstProjection, Projection secondProjection)
        {
            if (firstProjection == null) return secondProjection;
            if (secondProjection == null) return firstProjection;
            var Result = getProjectionForProjections(firstProjection, secondProjection);
            Result.Name = firstProjection.Name + "|" + secondProjection.Name;
            var temp = new SortedList();

            var pivList = new List<PropertyInProjection>();
            pivList.AddRange(firstProjection.Properties);

            foreach (var piv in secondProjection.Properties)
            {

                var index = pivList.FindIndex(p => p.Name == piv.Name);
                if (index >= 0)
                {
                    var pivRes = pivList[index];
                    pivRes.Visible = pivRes.Visible || piv.Visible;
                    pivRes.Caption = pivRes.Caption + " / " + piv.Caption;

                    pivList[index] = pivRes;
                }
                else
                {
                    pivList.Add(piv);
                }
            }
            Result.Properties = pivList.ToArray();



            //properties
            for (var i = 0; i < secondProjection.Properties.Length; i++)
                temp.Add(secondProjection.Properties[i].Name, secondProjection.Properties[i]);
            for (var i = 0; i < firstProjection.Properties.Length; i++)
                if (!temp.ContainsKey(firstProjection.Properties[i].Name))
                    temp.Add(firstProjection.Properties[i].Name, firstProjection.Properties[i]);
                else
                {
                    var p = (PropertyInProjection)temp[firstProjection.Properties[i].Name];
                    p.Visible = p.Visible || firstProjection.Properties[i].Visible;
                    p.Caption = p.Caption + " / " + firstProjection.Properties[i].Caption;
                    temp[firstProjection.Properties[i].Name] = p;
                }
            var props = new PropertyInProjection[temp.Count];
            temp.Values.CopyTo(props, 0);
            Result.Properties = props;


            //detail-properties
            temp.Clear();
            for (var i = 0; i < secondProjection.Details.Length; i++)
                temp.Add(secondProjection.Details[i].Name, secondProjection.Details[i]);
            for (var i = 0; i < firstProjection.Details.Length; i++)
                if (!temp.ContainsKey(firstProjection.Details[i].Name))
                    temp.Add(firstProjection.Details[i].Name, firstProjection.Details[i]);
                else
                {
                    var v = (DetailInProjection)temp[firstProjection.Details[i].Name];
                    v.LoadOnLoadAgregator = v.LoadOnLoadAgregator || firstProjection.Details[i].LoadOnLoadAgregator;
                    v.Projection = v.Projection | firstProjection.Details[i].Projection;
                    temp[firstProjection.Details[i].Name] = v;
                }
            var dets = new DetailInProjection[temp.Count];
            temp.Values.CopyTo(dets, 0);
            Result.Details = dets;
            return Result;

        }

        public static Projection operator &(Projection firstProjection, Projection secondProjection)
        {
            if (firstProjection == null || secondProjection == null) return null;
            var Result = getProjectionForProjections(firstProjection, secondProjection);
            Result.Name = firstProjection.ToString() + "&" + secondProjection.ToString();
            var temp = new SortedList();
            var temp1 = new SortedList();

            //properties
            for (var i = 0; i < secondProjection.Properties.Length; i++)
                temp.Add(secondProjection.Properties[i].Name, secondProjection.Properties[i]);
            for (var i = 0; i < firstProjection.Properties.Length; i++)
                if (temp.ContainsKey(firstProjection.Properties[i].Name))
                    temp1.Add(firstProjection.Properties[i].Name, firstProjection.Properties[i]);
            var props = new PropertyInProjection[temp1.Count];
            temp1.Values.CopyTo(props, 0);
            Result.Properties = props;

            //detail-properties
            temp.Clear();
            temp1.Clear();
            for (var i = 0; i < secondProjection.Details.Length; i++)
                temp.Add(secondProjection.Details[i].Name, secondProjection.Details[i]);
            for (var i = 0; i < firstProjection.Details.Length; i++)
                if (temp.ContainsKey(firstProjection.Details[i].Name))
                {
                    var div1 = (DetailInProjection)temp[firstProjection.Details[i].Name];
                    var div2 = firstProjection.Details[i];
                    var resdiv = new DetailInProjection()
                    {
                        Name = div1.Name,
                        Projection = div1.Projection & div1.Projection,
                        LoadOnLoadAgregator = div1.LoadOnLoadAgregator && div1.LoadOnLoadAgregator,
                        FormPath = div1.FormPath,
                        Caption = div1.Caption + " & " + div2.Caption,
                        Visible = div1.Visible && div2.Visible,
                        Order = div2.Order
                    };
                    temp1.Add(firstProjection.Details[i].Name, resdiv);
                }
            var dets = new DetailInProjection[temp1.Count];
            temp1.Values.CopyTo(dets, 0);
            Result.Details = dets;
            return Result;
        }

        public static Projection operator -(Projection firstProjection, Projection secondProjection)
        {

            var Result = getProjectionForProjections(firstProjection, secondProjection);
            Result.Name = firstProjection.ToString() + "-" + secondProjection.ToString();
            var temp = new SortedList();
            var temp1 = new SortedList();

            //properties
            for (var i = 0; i < firstProjection.Properties.Length; i++)
                temp.Add(firstProjection.Properties[i].Name, firstProjection.Properties[i]);
            for (var i = 0; i < secondProjection.Properties.Length; i++)
                if (temp.ContainsKey(secondProjection.Properties[i].Name))
                    temp.Remove(secondProjection.Properties[i].Name);
            var props = new PropertyInProjection[temp.Count];
            temp.Values.CopyTo(props, 0);
            Result.Properties = props;


            //detail-properties
            temp.Clear();
            temp1.Clear();

            for (var i = 0; i < firstProjection.Details.Length; i++)
                temp.Add(firstProjection.Details[i].Name, firstProjection.Details[i]);
            for (var i = 0; i < secondProjection.Details.Length; i++)
                if (temp.ContainsKey(secondProjection.Details[i].Name))
                    temp.Remove(secondProjection.Details[i].Name);
            var dets = new DetailInProjection[temp.Count];
            temp.Values.CopyTo(dets, 0);
            Result.Details = dets;
            return Result;
        }

        /// <summary>
        /// exclusive-OR
        /// </summary>
        /// <param name="firstProjection"></param>
        /// <param name="secondProjection"></param>
        /// <returns></returns>
        public static Projection operator ^(Projection firstProjection, Projection secondProjection)
        {
            var res = (firstProjection | secondProjection) - (firstProjection & secondProjection);
            res.Name = firstProjection.Name + "^" + secondProjection.Name;
            return res;
        }

        public int[] GetOrderedIndexes(string[] orderCols, string[] advCols, bool returnOrderIndexForPropery)
        {

            var DestProps = GetOrderedProperies(orderCols, advCols, out List<string> AllProps);
            if (returnOrderIndexForPropery)
                return AllProps.Select(x => DestProps.FindIndex(y => y == x)).ToArray();
            else
                return DestProps.Select(x => AllProps.FindIndex(y => y == x)).ToArray();
        }

        /// <summary>
        /// List of the properties in order of property define in the projection
        /// </summary>

        public List<string> GetOrderedProperies(string[] orderCols, string[] advCols, out List<string> DefaultProperties)
        {
            var AllProps = Properties.Select(x => x.Name).ToList();
            if (advCols != null) AllProps.AddRange(advCols);
            var DestProps = new List<string>();
            if (orderCols != null)
            {
                var errCols = orderCols.Where(x => !AllProps.Contains(x)).ToList();
                if (errCols.Count > 0)
                    throw new PlatformException($"Can't find ordering properties:({string.Join(',', errCols.ToArray())}) in ({string.Join(',', AllProps.ToArray())})");
                DestProps.AddRange(orderCols);
            }
            DestProps.AddRange(AllProps.Where(x => !DestProps.Contains(x)));
            DefaultProperties = AllProps;
            return DestProps;
        }

        /// <summary>
        /// Get field index in a projection starting from 0
        /// </summary>
        public int GetPropertyIndex(string PropertyName)
        {
            return Array.FindIndex(Properties, x => x.Name == PropertyName);
        }
    }
}
