namespace FluentExtractor
{
    using Extractors.Length;
    using Extractors.Required;
    using Results;

    public static class ExtractorsExtensions
    {
        #region Descriptors

        public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, bool isRequired = true)
        {
            return ruleBuilder.SetExtractor(new RequiredExtractor<T, TProperty>(isRequired));
        }

        public static IRuleBuilderOptions<T, string> Length<T>(this IRuleBuilder<T, string> ruleBuilder, int from, int to)
        {
            return ruleBuilder.SetExtractor(new LengthExtractor<T>(from, to));
        }

        public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int to)
        {
            return ruleBuilder.SetExtractor(new MaxLengthExtractor<T>(to));
        }

        public static IRuleBuilderOptions<T, string> MinLength<T>(this IRuleBuilder<T, string> ruleBuilder, int from)
        {
            return ruleBuilder.SetExtractor(new MinLengthExtractor<T>(from));
        }

        #endregion Descriptors

        #region Validation

        public static Metadata Extract<T>(this IExtractor<T> validator, string projection)
        {
            return validator.Extract(options => { options.SetProjection(projection); });
        }

        #endregion Validation
    }
}
