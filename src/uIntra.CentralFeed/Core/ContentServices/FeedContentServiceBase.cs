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

        public virtual IIntranetType GetFeedTabType(IPublishedContent content) => 
            GetActivityTypeFromPlugin(content, FeedPluginAlias);

        public virtual IIntranetType GetCreateActivityType(IPublishedContent content) => 
            GetActivityTypeFromPlugin(content, ActivityCreatePluginAlias);

        protected virtual  IIntranetType GetActivityTypeFromPlugin(IPublishedContent content, string gridPluginAlias)
        {
            var value = _gridHelper
                .GetValues(content, gridPluginAlias)
                .FirstOrDefault(t => t.value != null)
                .value;

            if (value == null)
                return _feedTypeProvider.Get(default(CentralFeedTypeEnum).ToInt());

            var tabTypeId = int.TryParse(value.tabType.ToString(), out int result)
                ? result
                : default(CentralFeedTypeEnum).ToInt();
            return _feedTypeProvider.Get(tabTypeId);
        }

    }
}
