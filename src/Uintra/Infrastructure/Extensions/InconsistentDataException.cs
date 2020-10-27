using System;

namespace Uintra.Infrastructure.Extensions
{
    public class InconsistentDataException : Exception
    {
        public InconsistentDataException(string message) : base(message)
        {
        }
    }
}