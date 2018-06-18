using System;
using Elmah;
using Uintra.Core.Exceptions;

namespace Compent.Uintra.Core.Exceptions
{
    public class ExceptionLogger: IExceptionLogger
    {
        public void Log(Exception ex)
        {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }
    }
}