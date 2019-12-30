using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compent.Extensions;
using UBaseline.Core.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Localization;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.Links.Models;
using Uintra20.Features.News.Converters.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.Converters
{
    public class NewsDetailsPageViewModelConverter : INodeViewModelConverter<NewsDetailsPageModel, NewsDetailsPageViewModel>
    {
        private readonly INewsService<Entities.News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;

        public NewsDetailsPageViewModelConverter(INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            IIntranetLocalizationService localizationService,
            ICommentsService commentsService,
            ILikesService likesService)
        {
            _newsService = newsService;
            _memberService = memberService;
            _localizationService = localizationService;
            _likesService = likesService;
            _commentsService = commentsService;
        }

        public void Map(NewsDetailsPageModel node, NewsDetailsPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.TryGetQueryValue<string>("id");

            if (Guid.TryParse(idStr, out Guid id))
            {
                viewModel.Details = GetDetails(id);
            }
        }

        private IntranetActivityDetailsViewModel GetDetails(Guid activityId)
        {
            IActivityLinks links = null;

            var newsItem = _newsService.Get(activityId);
            var detailsModel = newsItem.Map<IntranetActivityDetailsViewModel>();
            detailsModel.CanEdit = _newsService.CanEdit(newsItem);
            detailsModel.Links = links;
            detailsModel.Owner = _memberService.Get(newsItem).Map<MemberViewModel>();
            detailsModel.Type = _localizationService.Translate(newsItem.Type.ToString());
            var dates = newsItem.PublishDate.ToDateTimeFormat().ToEnumerable().ToList();
            if (newsItem.UnpublishDate.HasValue)
            {
                dates.Add(newsItem.UnpublishDate.Value.ToDateTimeFormat());
            }
            detailsModel.Dates = dates;
            DependencyResolver.Current.GetService<ILightboxHelper>().FillGalleryPreview(detailsModel, newsItem.MediaIds);

            return detailsModel;
        }
    }
}