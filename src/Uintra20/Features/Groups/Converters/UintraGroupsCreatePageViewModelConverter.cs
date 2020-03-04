using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsCreatePageViewModelConverter : UintraRestrictedNodeViewModelConverter<UintraGroupsCreatePageModel, UintraGroupsCreatePageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupService _groupService;

        public UintraGroupsCreatePageViewModelConverter(
            IMediaHelper mediaHelper,
            IGroupService groupService,
            IErrorLinksService errorLinksService)
        : base(errorLinksService)
        {
            _mediaHelper = mediaHelper;
            _groupService = groupService;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsCreatePageModel node, UintraGroupsCreatePageViewModel viewModel)
        {
            if (!_groupService.CanCreate())
            {
                return ForbiddenResult();
            }

            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;

            return OkResult();
        }
    }
}