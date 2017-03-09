using System;

namespace uCommunity.Core.App_Plugins.Core.Exceptions
{
    public class InconsistentDataException : Exception
    {
        public InconsistentDataException(string message) : base(message)
        {
        }
    }
}
