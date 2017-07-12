using System;
using Elmah;

namespace uIntra.Core.Exceptions
{
    public class ExceptionLogger: IExceptionLogger
    {
        public void Log(Exception ex)
        {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }
    }
}