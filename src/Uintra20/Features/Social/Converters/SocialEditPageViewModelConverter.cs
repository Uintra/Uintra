using System;
using System.Web;
using UBaseline.Core.Localization;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Links;
using Uintra20.Features.Social.Models;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Social.Converters
{
    public class SocialEditPageViewModelConverter
        : UintraRestrictedNodeViewModelConverter<SocialEditPageModel, SocialEditPageViewModel>
    {
        private readonly ILocalizationModelService _localizationModelService;
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IGroupHelper _groupHelper;

        public SocialEditPageViewModelConverter(
            ILocalizationModelService localizationModelService,
            ISocialService<Entities.Social> socialService,
            IUserTagService userTagService,
            ILightboxHelper lightboxHelper,
            IUserTagProvider userTagProvider,
            IFeedLinkService feedLinkService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService)
            : base(errorLinksService)
        {
            _localizationModelService = localizationModelService;
            _socialService = socialService;
            _userTagService = userTagService;
            _lightboxHelper = lightboxHelper;
            _userTagProvider = userTagProvider;
            _feedLinkService = feedLinkService;
            _groupHelper = groupHelper;
        }

        public override ConverterResponseModel MapViewModel(SocialEditPageModel node, SocialEditPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(id, out var parsedId)) return NotFoundResult();

            var social = _socialService.Get(parsedId);

            if (social == null)
            {
                return NotFoundResult();
            }

            if (!_socialService.CanEdit(parsedId))
            {
                return ForbiddenResult();
            }

            viewModel.CanDelete = _socialService.CanDelete(parsedId);
            viewModel.OwnerId = social.OwnerId;
            viewModel.Id = social.Id; //TODO Use link service to navigate from social edit page
            viewModel.Description = social.Description;
            viewModel.Name = _localizationModelService["Social.Edit"];
            viewModel.Tags = _userTagService.Get(parsedId);
            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, PresetStrategies.ForActivityDetails);
            viewModel.AvailableTags = _userTagProvider.GetAll();
            viewModel.Links = _feedLinkService.GetLinks(social.Id);

            var mediaSettings = _socialService.GetMediaSettings();
            viewModel.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;

            var groupIdStr = HttpContext.Current.Request["groupId"];
            if (Guid.TryParse(groupIdStr, out var groupId) && social.GroupId == groupId)
            {
                viewModel.GroupHeader = _groupHelper.GetHeader(groupId);
            }

            return OkResult();
        }
    }
}