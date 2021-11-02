using FluentExtractor.Descriptors;

namespace FluentExtractor
{
    public interface IRuleBuilderInitial<T, out TProperty> : IRuleBuilder<T, TProperty>
    {
    }

    public interface IRuleBuilder<T, out TProperty>
    {
        IRuleBuilderOptions<T, TProperty> SetExtractor(IExtractor<T, TProperty> extractor);
    }

    public interface IRuleBuilderOptions<T, out TProperty> : IRuleBuilder<T, TProperty>
    {
    }
}
