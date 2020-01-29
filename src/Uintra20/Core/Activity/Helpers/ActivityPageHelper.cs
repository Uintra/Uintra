using System;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;

namespace Uintra20.Core.Activity.Helpers
{
    public class ActivityPageHelper : IActivityPageHelper//TODO: Needs research
    {
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
            var pageAlias = _aliasProvider.GetDetailsPage(activityType);
            var detailsPageUrl = _nodeModelService.GetByAlias(pageAlias, _uBaselineRequestContext.Node.RootId)?.Url;
            
            return activityId.HasValue
                ? detailsPageUrl.AddIdParameter(activityId).ToLinkModel()
                : detailsPageUrl.ToLinkModel();
        }

        public UintraLinkModel GetCreatePageUrl(Enum activityType)
        {
            var pageAlias = _aliasProvider.GetCreatePage(activityType);
            var detailsPageUrl = _nodeModelService.GetByAlias(pageAlias, _uBaselineRequestContext.Node.RootId)?.Url;

            return detailsPageUrl.ToLinkModel();
        }

        public UintraLinkModel GetEditPageUrl(Enum activityType, Guid activityId)
        {
            var pageAlias = _aliasProvider.GetEditPage(activityType);
            var detailsPageUrl = _nodeModelService.GetByAlias(pageAlias, _uBaselineRequestContext.Node.RootId)?.Url;

            return detailsPageUrl.AddIdParameter(activityId).ToLinkModel();
        }
    }
}