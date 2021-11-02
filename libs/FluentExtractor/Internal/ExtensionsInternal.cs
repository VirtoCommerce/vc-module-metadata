namespace FluentExtractor.Internal
{
    using System;

    internal static class ExtensionsInternal
    {
        internal static void Guard(this object obj, string message, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        internal static void Guard(this string str, string message, string paramName)
        {
            if (str == null)
            {
                throw new ArgumentNullException(paramName, message);
            }

            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException(message, paramName);
            }
        }
    }
}
