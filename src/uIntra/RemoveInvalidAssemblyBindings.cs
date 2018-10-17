using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uintra
{
    public class RemoveInvalidAssemblyBindings : Task
    {
        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "Removing invalid assembly dependency bindings!");
            return true;
        }
    }
}