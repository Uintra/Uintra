using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using uCommunity.Core.User;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.Web
{
    public class MyLinksControllerBase : SurfaceController
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IMyLinksModelBuilder _myLinksModelBuilder;
        private readonly IMyLinksService _myLinksService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        protected virtual string MyLinksViewPath { get; } = "~/App_Plugins/Navigation/Links/View/Links.cshtml";
        protected virtual string MyLinkPageTitleNodePropertyAlias { get; } = string.Empty;

        public MyLinksControllerBase(
            UmbracoHelper umbracoHelper,
            IMyLinksModelBuilder myLinksModelBuilder,
            IMyLinksService myLinksService,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _umbracoHelper = umbracoHelper;
            _myLinksModelBuilder = myLinksModelBuilder;
            _myLinksService = myLinksService;
            _intranetUserService = intranetUserService;
        }

        public virtual PartialViewResult MyLinks()
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

        [HttpPost]
        public virtual ActionResult Add(int contentId)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            if (!_myLinksService.Exists(currentUser.Id, contentId))
            {
                return new EmptyResult();
            }

            _myLinksService.Create(currentUser.Id, contentId);

            var result = _myLinksModelBuilder.GetMenu();
            return PartialView(MyLinksViewPath, result);
        }

        [HttpPost]
        public virtual ActionResult Remove(Guid id)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var link = _myLinksService.Get(id);

            if (link == null || link.UserId != currentUser.Id)
            {
                return new EmptyResult();
            }

            _myLinksService.Delete(id);

            var result = _myLinksModelBuilder.GetMenu();
            return PartialView(MyLinksViewPath, result);
        }
    }
}
