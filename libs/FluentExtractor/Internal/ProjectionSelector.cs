namespace FluentExtractor.Internal
{
    public class ProjectionSelector : IExtractorSelector
    {
        private readonly string _projectionToExecute;
        public const string DefaultProjection = "default";

        public ProjectionSelector(string projection)
        {
            _projectionToExecute = projection;
        }

        public virtual bool CanExecute(IExtractionRule rule)
        {
            if (string.IsNullOrEmpty(rule.Projection) && string.IsNullOrEmpty(_projectionToExecute))
            {
                return true;
            }

            if (_projectionToExecute.Equals(DefaultProjection) && (string.IsNullOrEmpty(rule.Projection) || rule.Projection.Equals(DefaultProjection)))
            {
                return true;
            }

            return rule.Projection.Equals(_projectionToExecute);
        }
    }
}
