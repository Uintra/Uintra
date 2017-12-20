using System.Web.Mvc;
using uIntra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Users
{
    public class MemberEventService : IUmbracoEventService<IMemberService, DeleteEventArgs<IMember>>
    {
        public void Process(IMemberService sender, DeleteEventArgs<IMember> args)
        {
            var cacheableUserService = DependencyResolver.Current.GetService<ICacheableIntranetUserService>();
            var memberService = DependencyResolver.Current.GetService<IMemberService>();

            foreach (var member in args.DeletedEntities)
            {
                member.IsLockedOut = true;
                memberService?.Save(member);
                cacheableUserService?.UpdateUserCache(member.Key);
            }

            if (args.CanCancel)
            {
                args.Cancel = true;
            }
        }
    }
}
