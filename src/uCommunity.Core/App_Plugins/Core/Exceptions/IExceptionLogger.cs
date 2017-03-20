using System;

namespace uCommunity.Core.Exceptions
{
    interface IExceptionLogger
    {
        void Log(Exception ex);
    }
}
