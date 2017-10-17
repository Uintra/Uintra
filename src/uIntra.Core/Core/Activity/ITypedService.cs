using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public interface ITypedService
    {
        // TODO: rename to [ Type ]
        IIntranetType ActivityType { get; }
    }
}