using UBaseline.Core.Node;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Media;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsCreatePageViewModelConverter : INodeViewModelConverter<UintraGroupsCreatePageModel, UintraGroupsCreatePageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupService _groupService;

        public UintraGroupsCreatePageViewModelConverter(IMediaHelper mediaHelper, IGroupService groupService)
        {
            _mediaHelper = mediaHelper;
            _groupService = groupService;
        }

        public void Map(UintraGroupsCreatePageModel node, UintraGroupsCreatePageViewModel viewModel)
        {
            viewModel.CanCreate = _groupService.CanCreate();

            if (!viewModel.CanCreate)
            {
                return;
            }
            
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;
        }
    }
}