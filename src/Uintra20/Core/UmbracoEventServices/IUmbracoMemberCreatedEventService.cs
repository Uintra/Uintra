using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEventServices
{
    public interface IUmbracoMemberCreatedEventService
    {
        void ProcessMemberCreated(IMemberService sender, SaveEventArgs<IMember> args);
    }
}
