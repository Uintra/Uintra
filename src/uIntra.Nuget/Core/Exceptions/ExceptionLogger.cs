using System;
using Elmah;
using uIntra.Core.Exceptions;

namespace Compent.uIntra.Core.Exceptions
{
    public class ExceptionLogger: IExceptionLogger
    {
        public void Log(Exception ex)
        {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }
    }
}