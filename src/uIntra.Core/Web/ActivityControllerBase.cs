using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Web
{
    public abstract class ActivityControllerBase : SurfaceController
    {
        protected virtual string DetailsHeaderViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityDetailsHeader.cshtml";
        protected virtual string ItemHeaderViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityItemHeader.cshtml";
        protected virtual string OwnerEditViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityOwnerEdit.cshtml";
        protected virtual string PinActivityViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityPinView.cshtml";

        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IPermissionsService _permissionsService;

        protected ActivityControllerBase(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IPermissionsService permissionsService)
        {
            _intranetMemberService = intranetMemberService;
            _permissionsService = permissionsService;
        }

        public virtual ActionResult DetailsHeader(IntranetActivityDetailsHeaderViewModel header)
        {
            return PartialView(DetailsHeaderViewPath, header);
        }

        public virtual ActionResult ItemHeader(object header)
        {
            return PartialView(ItemHeaderViewPath, header);
        }

        public virtual ActionResult OwnerEdit(Guid ownerId, string ownerIdPropertyName, IntranetActivityTypeEnum activityType, IActivityCreateLinks links)
        {
            var model = new IntranetActivityOwnerEditModel
            {
                Owner = _intranetMemberService.Get(ownerId).Map<MemberViewModel>(),
                OwnerIdPropertyName = ownerIdPropertyName,
                Links = links
            };

            var currentMember = _intranetMemberService.GetCurrentMember();
            model.CanEditOwner = _permissionsService.IsRoleHasPermissions(currentMember.Role, PermissionConstants.CanEditOwner);
            if (model.CanEditOwner)
            {
                model.Members = GetUsersWithAccess(activityType, IntranetActivityActionEnum.Create);
            }

            return PartialView(OwnerEditViewPath, model);
        }

        public virtual ActionResult PinActivity(bool isPinned, DateTime? endPinDate)
        {
            return PartialView(PinActivityViewPath,
                new IntranetPinActivityModel
                {
                    IsPinned = isPinned,
                    EndPinDate = endPinDate ?? DateTime.Now
                });
        }

        protected virtual IEnumerable<IIntranetMember> GetUsersWithAccess(Enum activityType, IntranetActivityActionEnum action)
        {

            var result = _intranetMemberService
                .GetAll()
                .Where(member => _permissionsService.IsUserHasAccess(member, activityType, action))
                .OrderBy(user => user.DisplayedName);

            return result;
        }
    }
}