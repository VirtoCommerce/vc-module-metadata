namespace FluentExtractor.Internal
{
    public class DefaultExtractorSelector : IExtractorSelector
    {
        public bool CanExecute(IExtractionRule rule)
        {
            return string.IsNullOrEmpty(rule.Projection) || rule.Projection.Equals(ProjectionSelector.DefaultProjection);
        }
    }
}
