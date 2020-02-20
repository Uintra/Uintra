using UBaseline.Core.Node;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Media;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsCreatePageViewModelConverter : INodeViewModelConverter<UintraGroupsCreatePageModel, UintraGroupsCreatePageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IPermissionsService _permissionsService;

        public UintraGroupsCreatePageViewModelConverter(IMediaHelper mediaHelper, IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
            _mediaHelper = mediaHelper;
            _permissionsService = permissionsService;
        }

        public void Map(UintraGroupsCreatePageModel node, UintraGroupsCreatePageViewModel viewModel)
        {
            viewModel.CanCreate = _permissionsService.Check(new PermissionSettingIdentity(PermissionActionEnum.Create,
                PermissionResourceTypeEnum.Groups));

            if (!viewModel.CanCreate)
            {
                return;
            }
            
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;
        }
    }
}