using System;
using System.Linq;
using System.Web.Mvc;
using Compent.Extensions;
using Uintra.BaseControls;
using Uintra.Core.Constants;
using Uintra.Core.Extensions;
using Uintra.Core.Media;
using Uintra.Panels.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Uintra.Panels.Web
{
    public abstract class ContentPanelControllerBase : SurfaceController
    {
        protected virtual string ViewPath => @"~/App_Plugins/Panels/ContentPanel/content-panel.cshtml";

        private readonly IImageHelper _imageHelper;
        private readonly UmbracoHelper _umbracoHelper;

        protected ContentPanelControllerBase(IImageHelper imageHelper, UmbracoHelper umbracoHelper)
        {
            _imageHelper = imageHelper;
            _umbracoHelper = umbracoHelper;
        }

        public virtual ActionResult Render(ContentPanelModel model)
        {
            var viewModel = new ContentPanelViewModel
            {
                IsImportant = model.IsImportant,
                TitleLink = string.IsNullOrEmpty(Convert.ToString(model.TitleLink)) ? null : model.TitleLink.link,
                Title = model.Title,
                HasTitle = !string.IsNullOrEmpty(model.Title),
                ImageVideoSize = model.ImageVideoSize,
                Description = model.Description,
                LinksListTitle = model.LinksListTitle
            };

            viewModel.Target = viewModel.TitleLink.HasValue() ? model.TitleLink?.target : string.Empty;

            viewModel.HasMedia = model.Image.HasValue;
            viewModel.PosterImageUrl = viewModel.HasMedia ? _imageHelper.GetImageWithPreset(GetMediaUrl(model.Image.Value), UmbracoAliases.ImagePresets.Preview) : null;
            viewModel.VideoUrl = GetUrl(model.VideoLink);
            viewModel.ShowAsLightbox = model.VideoType == "lightbox";
            viewModel.VideoLinkAlternativeText = model.VideoLink.altText;

            if (!string.IsNullOrEmpty(viewModel.VideoUrl))
            {
                viewModel.VideoSourceType = (VideoSourceTypes)model.VideoLink.sourceType;
                viewModel.VideoTooltip = model.VideoLink.altText.ToString();
                viewModel.VideoSrc = string.Empty;
                viewModel.AutoplayVideo = "autoplay";
                viewModel.AutoplayIframe = "?autoplay=1";
            }

            if (viewModel.PosterImageUrl.HasValue())
            {
                viewModel.VideoSrc = model.VideoLink.embedUrl;
                viewModel.AutoplayVideo = string.Empty;
                viewModel.AutoplayIframe = string.Empty;
            }

            if (viewModel.VideoSourceType == VideoSourceTypes.Vimeo || viewModel.VideoSourceType == VideoSourceTypes.Youtube)
            {
                viewModel.EmbedUrl = model.VideoLink.embedUrl + viewModel.AutoplayIframe;
            }
            else
            {
                viewModel.EmbedUrl = model.VideoLink.embedUrl;
            }

            if (model.Links != null && model.Links.Count > 0)
            {
                foreach (var link in model.Links)
                {
                    viewModel.Links.Add(new ContentPanelLinkViewModel
                    {
                        Caption = link.caption,
                        Target = link.target,
                        Url = GetLinkUrl(link)
                    });
                }
            }

            if (model.Files != null)
            {
                var fileIds = model.Files.ToIntCollection();
                viewModel.Files = _umbracoHelper.TypedMedia(fileIds)
                    .Select(el => new ContentPanelFileViewModel
                    {
                        Url = el.Url,
                        Extension = el.GetMediaExtension().ToExtensionViewString(),
                        Name = el.Name
                    })
                    .ToList();
            }

            return View(ViewPath, viewModel);
        }

        protected virtual string GetMediaUrl(int mediaId)
        {
            var media = _umbracoHelper.TypedMedia(mediaId);
            return media?.Url;
        }

        protected virtual string GetUrl(dynamic media)
        {
            if (media != null && media.url != null && !string.IsNullOrEmpty(media.url.ToString()))
            {
                return media.url;
            }
            return string.Empty;
        }

        protected virtual string GetLinkUrl(dynamic link)
        {
            if (link.type == 0)
            {
                return _umbracoHelper.TypedContent((int)link.id)?.Url ?? link.link;
            }

            return link.link;
        }
    }
}
