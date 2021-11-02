namespace FluentExtractor.Internal
{
    public class ExtractorPicker<T>
    {
        private string _projection;

        internal ExtractorPicker()
        {
        }

        public ExtractorPicker<T> SetProjection(string projection)
        {
            if (!string.IsNullOrEmpty(projection))
            {
                _projection = projection;
            }

            return this;
        }

        private IExtractorSelector GetSelector() => _projection != null
            ? ExtractorOptions.Global.ExtractorSelectors.ProjectionExtractorSelectorFactory(_projection)
            : ExtractorOptions.Global.ExtractorSelectors.DefaultExtractorSelectorFactory();

        internal ExtractionContext<T> BuildContext()
        {
            return new ExtractionContext<T>(GetSelector());
        }
    }
}
