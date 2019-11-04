using System;
using Elmah;

namespace Uintra20.Core.Exceptions
{
    public class ExceptionLogger : IExceptionLogger
    {
        public void Log(Exception exception)
        {
            var elmahCon = ErrorLog.GetDefault(null);
            elmahCon.Log(new Error(exception));
        }
    }
}