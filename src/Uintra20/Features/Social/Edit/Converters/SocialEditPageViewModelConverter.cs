using System;
using System.Web;
using UBaseline.Core.Localization;
using UBaseline.Core.Node;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Social.Edit.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Social.Edit.Converters
{
    public class SocialEditPageViewModelConverter
        : INodeViewModelConverter<SocialEditPageModel, SocialEditPageViewModel>
    {
        private readonly ILocalizationModelService _localizationModelService;
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly ILightboxHelper _lightboxHelper;

        public SocialEditPageViewModelConverter(
            ILocalizationModelService localizationModelService,
            ISocialService<Entities.Social> socialService,
            IUserTagService userTagService,
            ILightboxHelper lightboxHelper, 
            IUserTagProvider userTagProvider)
        {
            _localizationModelService = localizationModelService;
            _socialService = socialService;
            _userTagService = userTagService;
            _lightboxHelper = lightboxHelper;
            _userTagProvider = userTagProvider;
        }

        public void Map(
            SocialEditPageModel node,
            SocialEditPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(id, out var parsedId)) return;
            
            viewModel.CanEdit = _socialService.CanEdit(parsedId);

            if (!viewModel.CanEdit)
            {
                return;
            }

            var social = _socialService.Get(parsedId);

            viewModel.OwnerId = social.OwnerId;
            viewModel.Id = social.Id; //TODO Use link service to navigate from social edit page
            viewModel.Description = social.Description;
            viewModel.Name = _localizationModelService["Social.Edit"];
            viewModel.Tags = _userTagService.Get(parsedId);
            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, PresetStrategies.ForActivityDetails);
            viewModel.AvailableTags = _userTagProvider.GetAll();

            var mediaSettings = _socialService.GetMediaSettings();
            viewModel.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;

            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");
            if (!Guid.TryParse(groupIdStr, out var groupId) || social.GroupId != groupId)
                return;

            viewModel.RequiresGroupHeader = true;
            viewModel.GroupId = groupId;
        }
    }
}