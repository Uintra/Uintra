using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
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
            var linkModels = _myLinksModelBuilder.GetMenu().ToList();
            var modelDTO = GetLinkDTO(CurrentPage.Id, Request.QueryString.ToString());

            var model = new MyLinksViewModel
            {
                ContentId = CurrentPage.Id,
                IsLinked = _myLinksService.Exists(modelDTO),
                Links = Mapper.Map<IEnumerable<MyLinkItemViewModel>>(linkModels)
            };

            return PartialView(MyLinksViewPath, model);
        }

        public virtual PartialViewResult List(IEnumerable<MyLinkItemViewModel> model)
        {
            return PartialView(MyLinksListViewPath, model);
        }

        [System.Web.Mvc.HttpPost]
        public virtual ActionResult Add([FromBody]int contentId)
        {
            var model = GetLinkDTO(contentId, Request.UrlReferrer.Query);

            if (_myLinksService.Exists(model))
            {
                throw new MyLinksDuplicatedException(model);
            }

            _myLinksService.Create(model);

            var linkModels = _myLinksModelBuilder.GetMenu();
            return PartialView(MyLinksListViewPath, Mapper.Map<IEnumerable<MyLinkItemViewModel>>(linkModels));
        }

        [System.Web.Mvc.HttpPost]
        public virtual ActionResult Remove([FromBody]int contentId)
        {
            var model = GetLinkDTO(contentId, Request.UrlReferrer.Query);

            if (!_myLinksService.Exists(model))
            {
                throw new MyLinksNotExistedException(model);
            }

            _myLinksService.Delete(model);

            var linkModels = _myLinksModelBuilder.GetMenu();
            return PartialView(MyLinksListViewPath, Mapper.Map<IEnumerable<MyLinkItemViewModel>>(linkModels));
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
