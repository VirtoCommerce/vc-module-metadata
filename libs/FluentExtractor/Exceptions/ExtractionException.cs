using System;

namespace FluentExtractor.Exceptions
{
    [Serializable]
    public class ExtractionException : Exception
    {
        public ExtractionException(string message) : base(message)
        {
        }
    }
}
