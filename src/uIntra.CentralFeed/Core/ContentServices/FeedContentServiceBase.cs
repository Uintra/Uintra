using System;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Grid;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public abstract class FeedContentServiceBase : IFeedContentService
    {
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IGridHelper _gridHelper;

        protected FeedContentServiceBase(IFeedTypeProvider feedTypeProvider, IGridHelper gridHelper)
        {
            _feedTypeProvider = feedTypeProvider;
            _gridHelper = gridHelper;
        }

        protected abstract string FeedPluginAlias { get; }
        protected abstract string ActivityCreatePluginAlias { get; }

        public virtual Enum GetFeedTabType(IPublishedContent content) =>
            GetCentralFeedTypeFromPlugin(content, FeedPluginAlias);

        public virtual Enum GetCreateActivityType(IPublishedContent content) =>
            GetCentralFeedTypeFromPlugin(content, ActivityCreatePluginAlias);

        protected virtual  Enum GetCentralFeedTypeFromPlugin(IPublishedContent content, string gridPluginAlias)
        {
            var value = _gridHelper
                .GetValues(content, gridPluginAlias)
                .FirstOrDefault(t => t.value != null)
                .value;

            if (value == null) return default(CentralFeedTypeEnum);

            var tabType = int.TryParse(value.tabType.ToString(), out int result)
                ? _feedTypeProvider[result]
                : default(CentralFeedTypeEnum);

            return tabType;
        }

    }
}
