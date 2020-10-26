using System;

namespace Uintra20.Infrastructure.Extensions
{
    public class InconsistentDataException : Exception
    {
        public InconsistentDataException(string message) : base(message)
        {
        }
    }
}