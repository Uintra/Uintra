using System;

namespace Uintra.Core.Exceptions
{
    public interface IExceptionLogger
    {
        void Log(Exception ex);
    }
}
