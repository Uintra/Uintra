using System;

namespace Uintra20.Infrastructure.Exceptions
{
    public interface IExceptionLogger
    {
        void Log(Exception ex);
    }
}
