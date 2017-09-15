using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public interface IActivityPageHelper
    {
        IIntranetType ActivityType { get; }
        string GetOverviewPageUrl();
        string GetDetailsPageUrl();
        string GetCreatePageUrl();
        string GetEditPageUrl();
    }
}