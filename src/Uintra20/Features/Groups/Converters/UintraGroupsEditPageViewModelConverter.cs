using System;
using System.Linq;
using System.Web;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Groups.Helpers;
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
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupHelper _groupHelper;

        public UintraGroupsEditPageViewModelConverter(
            IGroupService groupService,
            IMediaHelper mediaHelper,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService)
            : base(errorLinksService)
        {
            _groupService = groupService;
            _mediaHelper = mediaHelper;
            _groupHelper = groupHelper;
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
            viewModel.Info = _groupHelper.GetInfoViewModel(id);
            viewModel.GroupHeader = _groupHelper.GetHeader(id);
            viewModel.GroupId = id;

            return OkResult();
        }
    }
}