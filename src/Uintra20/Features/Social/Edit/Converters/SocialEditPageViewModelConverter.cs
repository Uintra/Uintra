using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Media.Strategies.ImageResize;
using Uintra20.Features.Social.Edit.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Social.Edit.Converters
{
    public class SocialEditPageViewModelConverter
        : INodeViewModelConverter<SocialEditPageModel, SocialEditPageViewModel>
    {
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IUserTagService _userTagService;
        private readonly ILightboxHelper _lightboxHelper;

        public SocialEditPageViewModelConverter(
            ISocialService<Entities.Social> socialService,
            IUserTagService userTagService,
            ILightboxHelper lightboxHelper)
        {
            _socialService = socialService;
            _userTagService = userTagService;
            _lightboxHelper = lightboxHelper;
        }

        public void Map(
            SocialEditPageModel node,
            SocialEditPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetUbaselineQueryValue("id");

            if (!Guid.TryParse(id, out var parsedId)) return;

            var social = _socialService.Get(parsedId);

            viewModel.Description = social.Description;
            viewModel.Tags = _userTagService.Get(parsedId);
            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, RenderStrategies.ForActivityDetails);
        }
    }
}