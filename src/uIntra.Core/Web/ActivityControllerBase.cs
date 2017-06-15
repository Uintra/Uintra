using System;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class ActivityControllerBase : SurfaceController
    {
        protected virtual string DetailsHeaderViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityDetailsHeader.cshtml";
        protected virtual string ItemHeaderViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityItemHeader.cshtml";
        protected virtual string CreatorEditViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityCreatorEdit.cshtml";
        protected virtual string PinActivityViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityPinView.cshtml";

        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IPermissionsService _permissionsService;

        protected ActivityControllerBase(IIntranetUserService<IIntranetUser> intranetUserService, IPermissionsService permissionsService)
        {
            _intranetUserService = intranetUserService;
            _permissionsService = permissionsService;
        }

        public virtual ActionResult DetailsHeader(IntranetActivityDetailsHeaderViewModel header)
        {
            return PartialView(DetailsHeaderViewPath, header);
        }

        public virtual ActionResult ItemHeader(IntranetActivityItemHeaderViewModel header)
        {
            return PartialView(ItemHeaderViewPath, header);
        }

        public virtual ActionResult CreatorEdit(IIntranetUser creator, string creatorIdPropertyName)
        {
            var model = new IntranetActivityCreatorEditModel
            {
                Creator = creator,
                CreatorIdPropertyName = creatorIdPropertyName
            };

            var currentUser = _intranetUserService.GetCurrentUser();
            model.CanEditCreator = _permissionsService.IsRoleHasPermissions(currentUser.Role, PermissionConstants.CanEditCreator);
            if (model.CanEditCreator)
            {
                model.Users = _intranetUserService.GetAll().OrderBy(user => user.DisplayedName);
            }

            return PartialView(CreatorEditViewPath, model);
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
    }
}