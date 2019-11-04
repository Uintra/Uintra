using System;

namespace Uintra20.Core.Exceptions
{
    public interface IExceptionLogger
    {
        void Log(Exception ex);
    }
}
