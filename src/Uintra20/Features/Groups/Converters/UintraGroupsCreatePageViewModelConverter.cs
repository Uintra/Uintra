using UBaseline.Core.Node;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Helpers;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsCreatePageViewModelConverter : UintraRestrictedNodeViewModelConverter<UintraGroupsCreatePageModel, UintraGroupsCreatePageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupService _groupService;
        private readonly IGroupHelper _groupHelper;

        public UintraGroupsCreatePageViewModelConverter(
            IMediaHelper mediaHelper,
            IGroupService groupService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService)
        : base(errorLinksService)
        {
            _mediaHelper = mediaHelper;
            _groupService = groupService;
            _groupHelper = groupHelper;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsCreatePageModel node, UintraGroupsCreatePageViewModel viewModel)
        {
            if (!_groupService.CanCreate())
            {
                return ForbiddenResult();
            }

            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.Navigation = _groupHelper.GroupNavigation();
            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;

            return OkResult();
        }
    }
}