using System.Web.Mvc;
using uIntra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Users
{
    public class MemberEventService : IUmbracoMemberDeletingEventService
    {
        private readonly ICacheableIntranetUserService _cacheableIntranetUserService;
        private readonly IMemberService _memberService;

        public MemberEventService(ICacheableIntranetUserService cacheableIntranetUserService, IMemberService memberService)
        {
            _cacheableIntranetUserService = cacheableIntranetUserService;
            _memberService = memberService;
        }

        public void ProcessMemberDeleting(IMemberService sender, DeleteEventArgs<IMember> args)
        {
            foreach (var member in args.DeletedEntities)
            {
                member.IsLockedOut = true;
                _memberService.Save(member);
                _cacheableIntranetUserService.UpdateUserCache(member.Key);
            }

            if (args.CanCancel)
            {
                args.Cancel = true;
            }
        }
    }
}
