using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using uCommunity.Core.User;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.Core.Exceptions;
using uCommunity.Navigation.DefaultImplementation;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.Web
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
            var currentPageLink = linkModels.Find(l => l.ContentId == CurrentPage.Id);

            var model = new MyLinksViewModel
            {
                Id = currentPageLink?.Id,
                ContentId = CurrentPage.Id,
                IsLinked = currentPageLink != null,
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
            var currentUser = _intranetUserService.GetCurrentUser();
            if (!_myLinksService.Exists(currentUser.Id, contentId))
            {
                throw new MyLinksDuplicatedException(currentUser.Id, contentId);
            }

            _myLinksService.Create(currentUser.Id, contentId);

            var result = _myLinksModelBuilder.GetMenu();
            return PartialView(MyLinksViewPath, result);
        }

        [System.Web.Mvc.HttpPost]
        public virtual ActionResult Remove([FromBody]Guid id)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var link = _myLinksService.Get(id);

            if (link == null || link.UserId != currentUser.Id)
            {
                throw new MyLinksNotExistedException(currentUser.Id, id);
            }

            _myLinksService.Delete(id);

            var result = _myLinksModelBuilder.GetMenu();
            return PartialView(MyLinksViewPath, result);
        }
    }
}
