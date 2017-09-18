using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public interface IActivityPageHelperFactory
    {
        IActivityPageHelper GetHelper(IIntranetType type, IEnumerable<string> baseXPath);
    }
}