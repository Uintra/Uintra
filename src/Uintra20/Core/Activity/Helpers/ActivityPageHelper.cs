using System;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Features.Links.Models;
using Uintra20.Features.News.Models;
using Uintra20.Features.Social.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;

namespace Uintra20.Core.Activity.Helpers
{
    public class ActivityPageHelper : IActivityPageHelper//TODO: Needs research
    {
        private const string NewsCreateName = "News Create";

        private readonly IDocumentTypeAliasProvider _aliasProvider;
        private readonly INodeModelService _nodeModelService;
        private readonly IUBaselineRequestContext _uBaselineRequestContext;

        public ActivityPageHelper(
            IDocumentTypeAliasProvider aliasProvider,
            INodeModelService nodeModelService,
            IUBaselineRequestContext uBaselineRequestContext)
        {
            _aliasProvider = aliasProvider;
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
                    detailsPageUrl = null;
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
            var pageAlias = _aliasProvider.GetCreatePage(activityType);

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
                        return null;
                    }
                default:
                    return null;
            }
        }

        public UintraLinkModel GetEditPageUrl(Enum activityType, Guid activityId)
        {
            var pageAlias = _aliasProvider.GetEditPage(activityType);
            var currentNode = _uBaselineRequestContext.Node;
            var detailsPageUrl = currentNode != null ? _nodeModelService.GetByAlias(pageAlias, currentNode.RootId)?.Url : null;

            return detailsPageUrl?.AddIdParameter(activityId)?.ToLinkModel();
        }
    }
}