using System;
using System.Web;
using Elmah;
using Uintra.Core.Exceptions;

namespace Compent.Uintra.Core.Exceptions
{
    public class ExceptionLogger: IExceptionLogger
    {
        public void Log(Exception exception)
        {
            var elmahCon = ErrorLog.GetDefault(null);
            elmahCon.Log(new Error(exception));
        }
    }
}