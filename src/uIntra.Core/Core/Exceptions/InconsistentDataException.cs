using System;

namespace Uintra.Core.Exceptions
{
    public class InconsistentDataException : Exception
    {
        public InconsistentDataException(string message) : base(message)
        {
        }
    }
}
