using System;

namespace Uintra20.Core.Search.Exceptions
{
    public class ElasticSearchRequestErrorException : Exception
    {
        public override string StackTrace { get; }

        public ElasticSearchRequestErrorException(string message, string stackTrace) : base(message)
        {
            StackTrace = stackTrace;
        }

        public override string ToString()
        {
            return $"{GetType()}:{Message}{Environment.NewLine}{StackTrace}";
        }
    }
}