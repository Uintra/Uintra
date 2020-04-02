using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UmbracoEvents.Services.Contracts;
using Uintra20.Core.User;
using Uintra20.Core.User.Models;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services.Implementations
{
    public class MemberEventService :
        IUmbracoMemberDeletingEventService,
        IUmbracoMemberCreatedEventService,
        IUmbracoMemberAssignedRolesEventService,
        IUmbracoMemberRemovedRolesEventService
    {
        private readonly ICacheableIntranetMemberService _cacheableIntranetMemberService;
        private readonly IMemberService _memberService;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public MemberEventService(
            ICacheableIntranetMemberService cacheableIntranetMemberService,
            IMemberService memberService,
            IIntranetUserService<IntranetUser> intranetUserService,
            IIntranetMemberGroupService intranetMemberGroupService,
            IIntranetMemberService<IntranetMember> intranetMemberService
        )
        {
            _cacheableIntranetMemberService = cacheableIntranetMemberService;
            _memberService = memberService;
            _intranetUserService = intranetUserService;
            _intranetMemberGroupService = intranetMemberGroupService;
            _intranetMemberService = intranetMemberService;
        }

        public void MemberUpdateHandler(
            IMemberService sender,
            SaveEventArgs<IMember> @event)
        {
            foreach (var member in @event.SavedEntities)
            {
                 _cacheableIntranetMemberService.UpdateMemberCache(member.Id);
                 EnableUser(member.Key);
            }
        }

        public void MemberCreateHandler(
            IMemberService sender,
            SaveEventArgs<IMember> @event)
        {
            foreach (var member in @event.SavedEntities)
            {
                _cacheableIntranetMemberService.UpdateMemberCache(member.Id);
            }
        }

        public void MemberDeleteHandler(
            IMemberService sender,
            DeleteEventArgs<IMember> @event)
        {
            foreach (var member in @event.DeletedEntities)
            {
                member.IsLockedOut = true;
                _memberService.Save(member);
                DisableUser(member.Key);

                _cacheableIntranetMemberService.UpdateMemberCache(member.Key);
            }

            if (@event.CanCancel) @event.Cancel = true;
        }
        public void MemberAssignedRolesHandler(
            IMemberService sender,
            RolesEventArgs @event)
        {
            @event.MemberIds.ForEach(memberId =>
            {
                _cacheableIntranetMemberService.UpdateMemberCache((int) memberId);
            });
        }

        public void MemberRemovedRolesHandler(
            IMemberService sender,
            RolesEventArgs @event)
        {
            @event.MemberIds.ForEach(memberId =>
            {
                _cacheableIntranetMemberService.UpdateMemberCache((int) memberId);
            });
        }
        
        private void DisableUser(Guid memberKey)
        {
            var member = _intranetMemberService.Get(memberKey);
            if (member.RelatedUser != null)
            {
                _intranetUserService.Disable(member.RelatedUser.Id);
            }
        }
        
        private void EnableUser(Guid memberKey)
        {
            var member = _intranetMemberService.Get(memberKey);
            if (!member.Inactive)
            {
                if (member.RelatedUser!=null && member.RelatedUser.IsLockedOut)
                {
                    _intranetUserService.Enable(member.RelatedUser.Id);    
                }
                
            }
        }
    }
}