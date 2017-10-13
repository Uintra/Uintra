using System.Linq;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public abstract class FeedContentHelperBase : IFeedContentHelper
    {
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IGridHelper _gridHelper;

        public abstract IIntranetType GetCreateActivityType(IPublishedContent content);

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
