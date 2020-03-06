using UBaseline.Core.Node;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Media;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsCreatePageViewModelConverter : INodeViewModelConverter<UintraGroupsCreatePageModel, UintraGroupsCreatePageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupService _groupService;
        private readonly IGroupHelper _groupHelper;

        public UintraGroupsCreatePageViewModelConverter(
            IMediaHelper mediaHelper, 
            IGroupService groupService,
            IGroupHelper groupHelper)
        {
            _mediaHelper = mediaHelper;
            _groupService = groupService;
            _groupHelper = groupHelper;
        }

        public void Map(UintraGroupsCreatePageModel node, UintraGroupsCreatePageViewModel viewModel)
        {
            viewModel.CanCreate = _groupService.CanCreate();

            if (!viewModel.CanCreate)
            {
                return;
            }
            
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.Navigation = _groupHelper.GroupNavigation();
            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;
        }
    }
}