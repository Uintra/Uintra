using System;
using System.Linq;
using System.Web;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsEditPageViewModelConverter : UintraRestrictedNodeViewModelConverter<UintraGroupsEditPageModel, UintraGroupsEditPageViewModel>
    {
        private readonly IGroupService _groupService;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IMediaHelper _mediaHelper;

        public UintraGroupsEditPageViewModelConverter(
            IGroupService groupService, 
            ILightboxHelper lightboxHelper, 
            IMediaHelper mediaHelper,
            IErrorLinksService errorLinksService)
        : base(errorLinksService)
        {
            _lightboxHelper = lightboxHelper;
            _groupService = groupService;
            _mediaHelper = mediaHelper;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsEditPageModel node, UintraGroupsEditPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return NotFoundResult();

            var group = _groupService.Get(id);

            if (group == null)
            {
                return NotFoundResult();
            }

            if (!_groupService.CanEdit(id))
            {
                return ForbiddenResult();
            }

            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;
            viewModel.Info = GetInfo(group);
            viewModel.GroupId = id;

            return OkResult();
        }

        public GroupInfoViewModel GetInfo(GroupModel group)
        {
            var groupInfo = group.Map<GroupInfoViewModel>();

            groupInfo.CanHide = _groupService.CanHide(group);

            if (group.ImageId.HasValue)
            {
                _lightboxHelper.FillGalleryPreview(groupInfo, Enumerable.Repeat(group.ImageId.Value, 1));
            }

            return groupInfo;
        }
    }
}