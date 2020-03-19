using System;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Features.Events.Models;
using Uintra20.Features.Links.Models;
using Uintra20.Features.News.Models;
using Uintra20.Features.Social.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Activity.Helpers
{
    public class ActivityPageHelper : IActivityPageHelper//TODO: Needs research
    {
        private readonly INodeModelService _nodeModelService;
        private readonly IUBaselineRequestContext _uBaselineRequestContext;

        public ActivityPageHelper(
            INodeModelService nodeModelService,
            IUBaselineRequestContext uBaselineRequestContext)
        {
            _nodeModelService = nodeModelService;
            _uBaselineRequestContext = uBaselineRequestContext;

        }

        public UintraLinkModel GetFeedUrl() =>
            _uBaselineRequestContext
                .HomeNode
                .Url
                .ToLinkModel();

        public UintraLinkModel GetDetailsPageUrl(Enum activityType, Guid? activityId = null)
        {
            var intranetActivityType = activityType is IntranetActivityTypeEnum @enum ? @enum : 0;
            UintraLinkModel detailsPageUrl;

            switch (intranetActivityType)
            {
                case IntranetActivityTypeEnum.News:
                {
                    var detailsPage = _nodeModelService.AsEnumerable()
                        .OfType<UintraNewsDetailsPageModel>()
                        .Single();

                    detailsPageUrl = detailsPage.Url.ToLinkModel();
                    break;
                }
                case IntranetActivityTypeEnum.Events:
                {
                    var detailsPage = _nodeModelService.AsEnumerable()
                        .OfType<EventDetailsPageModel>()
                        .Single();

                    detailsPageUrl = detailsPage.Url.ToLinkModel();
                    break;
                    }
                case IntranetActivityTypeEnum.Social:
                {
                    var detailsPage = _nodeModelService.AsEnumerable()
                        .OfType<SocialDetailsPageModel>()
                        .Single();

                    detailsPageUrl = detailsPage.Url.ToLinkModel();
                    break;
                }
                default:
                    detailsPageUrl = null;
                    break;
            }

            return activityId.HasValue
                ? detailsPageUrl?.AddParameter("id", activityId.ToString())
                : detailsPageUrl;
        }

        public UintraLinkModel GetCreatePageUrl(Enum activityType)
        {

            var intranetActivityType = activityType is IntranetActivityTypeEnum @enum ? @enum : 0;

            switch (intranetActivityType)
            {
                case IntranetActivityTypeEnum.News:
                {
                    var createPage = _nodeModelService.AsEnumerable()
                                    .OfType<UintraNewsCreatePageModel>()
                                    .Single();

                    return createPage.Url.ToLinkModel();
                }
                case IntranetActivityTypeEnum.Events:
                {
                    var createPage = _nodeModelService.AsEnumerable()
                        .OfType<EventCreatePageModel>()
                        .Single();

                    return createPage.Url.ToLinkModel();
                }
                default:
                    return null;
            }
        }

        public UintraLinkModel GetEditPageUrl(Enum activityType, Guid? activityId = null)
        {
            var intranetActivityType = activityType is IntranetActivityTypeEnum @enum ? @enum : 0;
            UintraLinkModel editPageUrl;

            switch (intranetActivityType)
            {
                case IntranetActivityTypeEnum.News:
                {
                    var editPage = _nodeModelService.AsEnumerable()
                        .OfType<UintraNewsEditPageModel>()
                        .Single();

                    editPageUrl = editPage.Url.ToLinkModel();
                    break;
                }
                case IntranetActivityTypeEnum.Events:
                {
                    var editPage = _nodeModelService.AsEnumerable()
                        .OfType<EventEditPageModel>()
                        .Single();

                    editPageUrl = editPage.Url.ToLinkModel();
                    break;
                }
                case IntranetActivityTypeEnum.Social:
                {
                    var editPage = _nodeModelService.AsEnumerable()
                        .OfType<SocialEditPageModel>()
                        .Single();

                    editPageUrl = editPage.Url.ToLinkModel();
                    break;
                }
                default:
                    editPageUrl = null;
                    break;
            }

            return activityId.HasValue
                ? editPageUrl?.AddParameter("id", activityId.ToString())
                : editPageUrl;
        }
    }
}