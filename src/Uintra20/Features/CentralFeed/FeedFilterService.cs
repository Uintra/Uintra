using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Features.CentralFeed.Entities;
using Uintra20.Features.CentralFeed.Models.Feed;
using Uintra20.Features.Subscribe;

namespace Uintra20.Features.CentralFeed
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