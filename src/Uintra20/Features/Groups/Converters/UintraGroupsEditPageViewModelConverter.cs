using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Groups.Links;
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
        private readonly IGroupLinkProvider _groupLinkProvider;

        public UintraGroupsEditPageViewModelConverter(IGroupService groupService, ILightboxHelper lightboxHelper, IMediaHelper mediaHelper, IGroupLinkProvider groupLinkProvider)
        {
            _lightboxHelper = lightboxHelper;
            _groupService = groupService;
            _mediaHelper = mediaHelper;
            _groupLinkProvider = groupLinkProvider;
        }

        public void Map(UintraGroupsEditPageModel node, UintraGroupsEditPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;

            var canEdit = _groupService.CanEdit(id);

            if (!canEdit)
            {
                return;
            }

            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;
            viewModel.Info = GetInfo(id);
            viewModel.Links = _groupLinkProvider.GetGroupLinks(id, canEdit);
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