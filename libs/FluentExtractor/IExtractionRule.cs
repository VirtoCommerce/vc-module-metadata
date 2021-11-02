using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentExtractor.Descriptors;

namespace FluentExtractor
{
    public interface IValidationRule<T, TProperty> : IExtractionRule
    {
        void SetExtractor(IExtractor<T, TProperty> validator);
    }

    public interface IExtractionRule
    {
        string Projection { get; set; }

        public string PropertyName { get; set; }
    }
}
