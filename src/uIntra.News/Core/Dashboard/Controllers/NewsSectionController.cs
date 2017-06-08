using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Web.WebApi;

namespace uIntra.News.Dashboard
{
    public class NewsSectionController : UmbracoAuthorizedApiController
    {
        private readonly INewsService<NewsBase> _newsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public NewsSectionController(INewsService<NewsBase> newsService, IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _newsService = newsService;
            _intranetUserService = intranetUserService;
        }

        public IEnumerable<NewsBackofficeViewModel> GetAll()
        {
            var news = _newsService.GetAll(true);
            foreach (var n in news)
            {
                n.CreatorId = _intranetUserService.GetCreator(n).Id;
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
            result.CreatorId = _intranetUserService.GetCreator(createdModel).Id;
            return result;
        }

        [HttpPost]
        public NewsBackofficeViewModel Save(NewsBackofficeSaveModel saveModel)
        {
            _newsService.Save(saveModel.Map<NewsBase>());
            var updatedModel = _newsService.Get(saveModel.Id);
            var result = updatedModel.Map<NewsBackofficeViewModel>();
            result.CreatorId = _intranetUserService.GetCreator(updatedModel).Id;
            return result;
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _newsService.Delete(id);
        }
    }
}