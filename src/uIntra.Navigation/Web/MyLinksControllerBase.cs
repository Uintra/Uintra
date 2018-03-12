using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Mvc;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Navigation.Exceptions;
using Uintra.Navigation.MyLinks;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Uintra.Navigation.Web
{
    public abstract class MyLinksControllerBase : SurfaceController
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IMyLinksModelBuilder _myLinksModelBuilder;
        private readonly IMyLinksService _myLinksService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;

        protected virtual string MyLinksViewPath { get; } = "~/App_Plugins/Navigation/MyLinks/View/MyLinks.cshtml";
        protected virtual string MyLinksListViewPath { get; } = "~/App_Plugins/Navigation/MyLinks/View/MyLinksList.cshtml";

        protected MyLinksControllerBase(
            UmbracoHelper umbracoHelper,
            IMyLinksModelBuilder myLinksModelBuilder,
            IMyLinksService myLinksService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IActivityTypeProvider activityTypeProvider)
        {
            _umbracoHelper = umbracoHelper;
            _myLinksModelBuilder = myLinksModelBuilder;
            _myLinksService = myLinksService;
            _intranetUserService = intranetUserService;
            _activityTypeProvider = activityTypeProvider;
            _documentTypeAliasProvider = documentTypeAliasProvider;
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
            var linkModels = _myLinksModelBuilder.GetMenu().ToList();
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
            if (IsActivityLink(contentId))
            {
                model.ActivityId = GetActivityLinkFromQuery(queryString);
            }

            if (IsGroupPage(contentId))
            {
                model.ActivityId = GetActivityLinkFromQuery(queryString);
            }

            return model;
        }

        protected bool IsActivityLink(int contentId)
        {
            var page = _umbracoHelper.TypedContent(contentId);
            foreach (var type in _activityTypeProvider.All)
            {
                if (page.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetDetailsPage(type)) ||
                    page.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetEditPage(type)))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool IsGroupPage(int contentId)
        {
            var page = _umbracoHelper.TypedContent(contentId);

            return page.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetGroupRoomPage());
        }

        protected Guid? GetActivityLinkFromQuery(string query)
        {
            var activityIdMatch = Regex.Match(query, @"id=([0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12})", RegexOptions.IgnoreCase);

            if (activityIdMatch.Success)
            {
                return new Guid(activityIdMatch.Groups[1].Value);
            }
            return null;
        }

        protected Guid? GetGroupLinkFromQuery(string query)
        {
            var groupIdMatch = Regex.Match(query, @"groupId=([0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12})", RegexOptions.IgnoreCase);

            if (groupIdMatch.Success)
            {
                return new Guid(groupIdMatch.Groups[1].Value);
            }
            return null;
        }

    }
}
