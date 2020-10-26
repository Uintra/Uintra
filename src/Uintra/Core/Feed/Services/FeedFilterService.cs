using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Settings;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Subscribe;

namespace Uintra.Core.Feed.Services
{
    public class FeedFilterService : IFeedFilterService
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly ISubscribeService _subscribeService;

        public FeedFilterService(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ISubscribeService subscribeService)
        {
            _intranetMemberService = intranetMemberService;
            _subscribeService = subscribeService;
        }

        public virtual IEnumerable<IFeedItem> ApplyFilters(IEnumerable<IFeedItem> items, FeedFilterStateModel filterState, FeedSettings settings)
        {
            if (filterState.ShowSubscribed.GetValueOrDefault() && settings.HasSubscribersFilter)
            {
                items = items.Where(i =>
                    i is ISubscribable subscribable &&
                    _subscribeService.IsSubscribed(_intranetMemberService.GetCurrentMember().Id, subscribable));
            }

            if (filterState.ShowPinned.GetValueOrDefault() && settings.HasPinnedFilter)
            {
                items = items.Where(i => i.IsPinned);
            }

            items = ApplyAdditionalFilters(items);
            return items;
        }

        public virtual IEnumerable<IFeedItem> ApplyAdditionalFilters(IEnumerable<IFeedItem> feedItems)
        {
            return feedItems;
        }
    }
}