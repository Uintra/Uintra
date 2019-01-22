using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Uintra.Core.Extensions;
using Uintra.Core.Media;
using Uintra.Core.User;
using Umbraco.Web.WebApi;

namespace Uintra.News.Dashboard
{
    public abstract class NewsSectionControllerBase : UmbracoAuthorizedApiController
    {
        private readonly INewsService<NewsBase> _newsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IMediaHelper _mediaHelper;

        protected NewsSectionControllerBase(INewsService<NewsBase> newsService, IIntranetMemberService<IIntranetMember> intranetMemberService, IMediaHelper mediaHelper)
        {
            _newsService = newsService;
            _intranetMemberService = intranetMemberService;
            _mediaHelper = mediaHelper;
        }

        public IEnumerable<NewsBackofficeViewModel> GetAll()
        {
            var news = _newsService.GetAll(true);
            var result = news.Map<IEnumerable<NewsBackofficeViewModel>>().OrderByDescending(el => el.ModifyDate);
            return result;
        }

        [HttpPost]
        public virtual NewsBackofficeViewModel Create(NewsBackofficeCreateModel createModel)
        {
            var creatingNews = createModel.Map<NewsBase>();
            creatingNews.CreatorId = _intranetMemberService.GetCurrentMemberId();
            var newsId = _newsService.Create(creatingNews);

            var createdNews = _newsService.Get(newsId);
            var result = createdNews.Map<NewsBackofficeViewModel>();
            return result;
        }

        [HttpPost]
        public virtual NewsBackofficeViewModel Save(NewsBackofficeSaveModel saveModel)
        {
            var news = _newsService.Get(saveModel.Id);
            news = Mapper.Map(saveModel, news);
            _newsService.Save(news);
            _mediaHelper.RestoreMedia(news.MediaIds);

            var updatedModel = _newsService.Get(saveModel.Id);
            var result = updatedModel.Map<NewsBackofficeViewModel>();
            return result;
        }

        [HttpDelete]
        public virtual void Delete(Guid id)
        {
            _newsService.Delete(id);
        }
    }
}