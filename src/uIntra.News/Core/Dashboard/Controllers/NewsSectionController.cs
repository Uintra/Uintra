using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;
using Umbraco.Web.WebApi;

namespace uIntra.News.Dashboard
{
    public class NewsSectionController : UmbracoAuthorizedApiController
    {
        private readonly INewsService<NewsBase> _newsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IMediaHelper _mediaHelper;

        public NewsSectionController(INewsService<NewsBase> newsService, IIntranetUserService<IIntranetUser> intranetUserService, IMediaHelper mediaHelper)
        {
            _newsService = newsService;
            _intranetUserService = intranetUserService;
            _mediaHelper = mediaHelper;
        }

        public IEnumerable<NewsBackofficeViewModel> GetAll()
        {
            var news = _newsService.GetAll(true);
            foreach (var n in news)
            {
                n.CreatorId = _intranetUserService.Get(n).Id;
            }

            var result = news.Map<IEnumerable<NewsBackofficeViewModel>>().OrderByDescending(el => el.ModifyDate);
            return result;
        }

        [HttpPost]
        public NewsBackofficeViewModel Create(NewsBackofficeCreateModel createModel)
        {
            var newsId = _newsService.Create(createModel.Map<NewsBase>());
            var createdModel = _newsService.Get(newsId);
            var result = createdModel.Map<NewsBackofficeViewModel>();
            result.CreatorId = _intranetUserService.Get(createdModel).Id;
            return result;
        }

        [HttpPost]
        public NewsBackofficeViewModel Save(NewsBackofficeSaveModel saveModel)
        {
            var news = _newsService.Get(saveModel.Id);
            news = Mapper.Map(saveModel, news);
            _newsService.Save(news);
            _mediaHelper.RestoreMedia(news.MediaIds);

            var updatedModel = _newsService.Get(saveModel.Id);
            var result = updatedModel.Map<NewsBackofficeViewModel>();
            result.CreatorId = _intranetUserService.Get(updatedModel).Id;
            return result;
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _newsService.Delete(id);
        }
    }
}