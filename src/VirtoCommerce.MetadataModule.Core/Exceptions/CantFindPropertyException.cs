using System;

namespace VirtoCommerce.MetadataModule.Core.Exceptions
{
    public class CantFindPropertyException : Exception
    {
        public string PropertyName { get; set; }
        public Type ClassType { get; set; }
        public CantFindPropertyException(string prop, Type type) : base("Cant find property <" + prop + "> in the class " + type.FullName)
        {
            PropertyName = prop;
            ClassType = type;
        }
    }

}
