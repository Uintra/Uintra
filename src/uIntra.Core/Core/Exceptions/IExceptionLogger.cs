using System;

namespace uIntra.Core.Exceptions
{
    public interface IExceptionLogger
    {
        void Log(Exception ex);
    }
}
