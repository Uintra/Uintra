using System;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.PanelContainer;
using Uintra20.Core.Activity.Models;
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
            var currentNode = _uBaselineRequestContext.Node;
            var detailsPageUrl = currentNode != null ? _nodeModelService.GetByAlias(pageAlias, currentNode.RootId)?.Url : null;
            
            return activityId.HasValue
                ? detailsPageUrl?.AddIdParameter(activityId).ToLinkModel()
                : detailsPageUrl?.ToLinkModel();
        }

        public UintraLinkModel GetCreatePageUrl(Enum activityType)
        {
            var createUrl = _nodeModelService
                .AsEnumerable()
                .Where(n => n is IPanelsComposition panel)
                .FirstOrDefault(n =>
                {
                    var panels = ((IPanelsComposition)n).Panels.Value.Panels;
                    var isLocalActivityCreate = panels
                        .OfType<LocalPanelModel>()
                        .Any(lp => lp.Node is ActivityCreatePanelModel ac && ac.TabType.Value == activityType.ToString());

                    var isGlobalActivityCreate = panels
                        .OfType<GlobalPanelModel>()
                        .Any(lp => _nodeModelService.Get(lp.NodeId) is ActivityCreatePanelModel ac && ac.TabType.Value == activityType.ToString());

                    return isLocalActivityCreate || isGlobalActivityCreate;
                })
                ?.Url;

            return createUrl.ToLinkModel();
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