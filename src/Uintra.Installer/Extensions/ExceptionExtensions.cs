using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Installer.Extensions
{
    public static class ExceptionExtensions
    {
        public static string FormatForLogging(this Exception ex)
        {
            return ex.Message + "\n" + (object)ex.TargetSite + "\n" + ex.StackTrace;
        }
    }
}
