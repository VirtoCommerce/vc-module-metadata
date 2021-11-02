namespace FluentExtractor.Internal
{
    public interface IExtractorSelector
    {
        bool CanExecute(IExtractionRule rule);
    }
}
