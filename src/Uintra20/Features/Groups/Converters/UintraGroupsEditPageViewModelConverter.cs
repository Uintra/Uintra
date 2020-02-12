using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsEditPageViewModelConverter : INodeViewModelConverter<UintraGroupsEditPageModel, UintraGroupsEditPageViewModel>
    {
        private readonly IGroupService _groupService;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IMediaHelper _mediaHelper;

        public UintraGroupsEditPageViewModelConverter(IGroupService groupService, ILightboxHelper lightboxHelper, IMediaHelper mediaHelper)
        {
            _lightboxHelper = lightboxHelper;
            _groupService = groupService;
            _mediaHelper = mediaHelper;
        }

        public void Map(UintraGroupsEditPageModel node, UintraGroupsEditPageViewModel viewModel)
        {
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;

            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;

            viewModel.Info = GetInfo(id);
        }

        public GroupInfoViewModel GetInfo(Guid groupId)
        {
            var group = _groupService.Get(groupId);

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