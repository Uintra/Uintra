using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using uIntra.Navigation.Exceptions;
using uIntra.Navigation.MyLinks;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Navigation.Web
{
    public abstract class MyLinksControllerBase : SurfaceController
    {
        private readonly IMyLinksModelBuilder _myLinksModelBuilder;
        private readonly IMyLinksService _myLinksService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        protected virtual string MyLinksViewPath { get; } = "~/App_Plugins/Navigation/MyLinks/View/MyLinks.cshtml";
        protected virtual string MyLinksListViewPath { get; } = "~/App_Plugins/Navigation/MyLinks/View/MyLinksList.cshtml";

        protected MyLinksControllerBase(
            UmbracoHelper umbracoHelper,
            IMyLinksModelBuilder myLinksModelBuilder,
            IMyLinksService myLinksService,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _myLinksModelBuilder = myLinksModelBuilder;
            _myLinksService = myLinksService;
            _intranetUserService = intranetUserService;
        }

        public virtual PartialViewResult Overview()
        {
            var dto = GetLinkDTO(CurrentPage.Id, Request.QueryString.ToString());
            var currentContentLinkId = _myLinksService.Get(dto)?.Id;

            var model = new MyLinksViewModel
            {
                ContentId = CurrentPage.Id,
                CurrentMyLinkId = currentContentLinkId,
            };

            return PartialView(MyLinksViewPath, model);
        }

        public virtual PartialViewResult List()
        {
            return PartialView(MyLinksListViewPath, GetMyLinkItemViewModel());
        }

        [System.Web.Mvc.HttpPost]
        public virtual JsonResult Add([FromBody]int contentId)
        {
            var model = GetLinkDTO(contentId, Request.UrlReferrer.Query);

            if (_myLinksService.Get(model) != null)
            {
                throw new MyLinksDuplicatedException(model);
            }

            var id = _myLinksService.Create(model);

            return Json(new { Id = id });
        }

        [System.Web.Mvc.HttpDelete]
        public virtual void Remove(Guid id)
        {
            _myLinksService.Delete(id);
        }

        protected virtual IEnumerable<MyLinkItemViewModel> GetMyLinkItemViewModel()
        {
            var linkModels = _myLinksModelBuilder.GetMenu();
            return linkModels.Map<IEnumerable<MyLinkItemViewModel>>();
        }

        protected virtual MyLinkDTO GetLinkDTO(int contentId, string queryString)
        {
            var model = new MyLinkDTO
            {
                ContentId = contentId,
                UserId = _intranetUserService.GetCurrentUser().Id,
                QueryString = queryString
            };

            return model;
        }
    }
}
