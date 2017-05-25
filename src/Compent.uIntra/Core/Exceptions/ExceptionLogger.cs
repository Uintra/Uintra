using System;
using Elmah;
using uCommunity.Core.Exceptions;

namespace Compent.uCommunity.Core.Exceptions
{
    public class ExceptionLogger: IExceptionLogger
    {
        public void Log(Exception ex)
        {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }
    }
}