using UBaseline.Core.Node;
using Uintra.Core.Localization;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Features.Tagging.UserTags.Converters
{
    public class UserTagsPanelViewModelConverter : INodeViewModelConverter<UserTagsPanelModel, UserTagsPanelViewModel>
    {
        private readonly IIntranetLocalizationService _intranetLocalizationService;

        public UserTagsPanelViewModelConverter(
            IIntranetLocalizationService intranetLocalizationService)
        {
            _intranetLocalizationService = intranetLocalizationService;
        }

        public void Map(UserTagsPanelModel node, UserTagsPanelViewModel viewModel)
        {
            viewModel.Title = _intranetLocalizationService.Translate("UserTagsView.Title.lbl");
        }
    }
}