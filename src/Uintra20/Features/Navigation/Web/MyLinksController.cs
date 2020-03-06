using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Navigation.Models;
using Uintra20.Features.Navigation.Models.MyLinks;
using Uintra20.Features.Navigation.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Navigation.Web
{
    public class MyLinksController : UBaselineApiController
    {
        private readonly IMyLinksHelper _myLinksHelper;
        private readonly IMyLinksService _myLinksService;
        private readonly IUBaselineRequestContext _uBaselineRequestContext;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public MyLinksController(
            IMyLinksHelper myLinksHelper,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IMyLinksService myLinksService,
            IUBaselineRequestContext uBaselineRequestContext)
        {
            _myLinksHelper = myLinksHelper;
            _intranetMemberService = intranetMemberService;
            _myLinksService = myLinksService;
            _uBaselineRequestContext = uBaselineRequestContext;
        }

        [HttpPost]
        public virtual async Task<IEnumerable<MyLinkItemViewModel>> Add()
        {
            var contentId = _uBaselineRequestContext.Node.Id;

            var model = GetLinkDto(contentId, HttpContext.Current.Request.UrlReferrer?.Query);

            if (await _myLinksService.GetAsync(model) != null)
            {
                return await GetMyLinkItemViewModelAsync();
            }

            await _myLinksService.CreateAsync(model);

            return await GetMyLinkItemViewModelAsync();
        }

        [HttpDelete]
        public virtual async Task<IEnumerable<MyLinkItemViewModel>> Remove(Guid id)
        {
            await _myLinksService.DeleteAsync(id);

            return await GetMyLinkItemViewModelAsync();
        }

        protected virtual MyLinkDTO GetLinkDto(int contentId, string queryString)
        {
            if (queryString == null)
            {
                queryString = string.Empty;
            }

            var model = new MyLinkDTO
            {
                ContentId = contentId,
                UserId = _intranetMemberService.GetCurrentMember().Id,
                QueryString = queryString
            };
            if (_myLinksHelper.IsActivityLink(contentId))
            {
                model.ActivityId = GetActivityLinkFromQuery(queryString);
            }

            if (_myLinksHelper.IsGroupPage(contentId))
            {
                model.ActivityId = GetActivityLinkFromQuery(queryString);
            }

            return model;
        }

        protected Guid? GetActivityLinkFromQuery(string query)
        {
            var activityIdMatch = HttpUtility.ParseQueryString(query).Get("id");

            if (Guid.TryParse(activityIdMatch, out Guid result))
            {
                return result;
            }

            return null;
        }

        protected virtual IEnumerable<MyLinkItemViewModel> GetMyLinkItemViewModel()
        {
            var linkModels = _myLinksHelper.GetMenu();
            return linkModels.Map<IEnumerable<MyLinkItemViewModel>>();
        }

        protected virtual async Task<IEnumerable<MyLinkItemViewModel>> GetMyLinkItemViewModelAsync()
        {
            var linkModels = await _myLinksHelper.GetMenuAsync();
            return linkModels.Map<IEnumerable<MyLinkItemViewModel>>();
        }
    }
}