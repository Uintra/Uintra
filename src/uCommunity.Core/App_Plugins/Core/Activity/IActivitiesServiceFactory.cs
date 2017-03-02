namespace uCommunity.Core.App_Plugins.Core.Activity
{
    public interface IActivitiesServiceFactory
    {
        IIntranetActivityItemServiceBase GetService(IntranetActivityTypeEnum type);
    }
}
