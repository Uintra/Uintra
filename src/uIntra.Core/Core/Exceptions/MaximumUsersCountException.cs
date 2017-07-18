using System;

namespace uIntra.Core.Exceptions
{
    public class MaximumUsersCountException : Exception
    {
        public MaximumUsersCountException(string message) : base(message)
        {
        }
    }
}
