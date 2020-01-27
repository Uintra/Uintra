using Compent.Extensions;
using System;
using System.Web;
using UBaseline.Core.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Localization;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.News.Models;
using Uintra20.Features.Social.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.Converters
{
    public class UintraNewsDetailsPageViewModelConverter : INodeViewModelConverter<UintraNewsDetailsPageModel, UintraNewsDetailsPageViewModel>
    {
        private readonly IFeedLinkService _feedLinkService;
        private readonly INewsService<Entities.News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IIntranetLocalizationService _localizationService;

        public UintraNewsDetailsPageViewModelConverter(
            IFeedLinkService feedLinkService,
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            IIntranetLocalizationService localizationService)
        {
            _feedLinkService = feedLinkService;
            _newsService = newsService;
            _memberService = memberService;
            _localizationService = localizationService;
        }

        public void Map(UintraNewsDetailsPageModel node, UintraNewsDetailsPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.TryGetQueryValue<string>("id");

            if (Guid.TryParse(idStr, out var id))
                viewModel.Details = GetDetails(id);
            
        }

        private NewsViewModel GetDetails(Guid activityId)
        {
            var news = _newsService.Get(activityId);

            var details = news.Map<NewsViewModel>();

            details.Media = MediaHelper.GetMediaUrls(news.MediaIds);
            
            details.CanEdit = _newsService.CanEdit(news);
            details.Links = _feedLinkService.GetLinks(activityId);
            details.IsReadOnly = false;
            details.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            details.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerable();
            details.HeaderInfo.Owner = _memberService.Get(news).Map<MemberViewModel>();
            details.HeaderInfo.Links = _feedLinkService.GetLinks(activityId);

            return details;
        }
    }
}