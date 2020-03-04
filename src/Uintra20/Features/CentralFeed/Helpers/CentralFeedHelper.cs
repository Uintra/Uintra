using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Extensions;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Providers;
using Uintra20.Features.CentralFeed.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.CentralFeed.Helpers
{
    public class CentralFeedHelper : ICentralFeedHelper
    {
        private readonly ICentralFeedService _centralFeedService;
        private readonly ICentralFeedContentProvider _contentProvider;
        private readonly IGroupFeedService _groupFeedService;

        public CentralFeedHelper(
            IActivitiesServiceFactory activitiesServiceFactory,
            ICentralFeedService centralFeedService,
            
            ICentralFeedContentProvider contentProvider,
            IGroupFeedService groupFeedService
            )
        {
            _centralFeedService = centralFeedService;
            activitiesServiceFactory.GetServices<IIntranetActivityService<IIntranetActivity>>();
            _contentProvider = contentProvider;
            _groupFeedService = groupFeedService;
        }

        public IEnumerable<IFeedItem> GetCentralFeedItems(Enum centralFeedType)
        {
            return centralFeedType is CentralFeedTypeEnum.All
                ? _centralFeedService.GetFeed().OrderByDescending(item => item.PublishDate)
                : _centralFeedService.GetFeed(centralFeedType);
        }

        public IEnumerable<IFeedItem> GetGroupFeedItems(Enum type, Guid groupId)
        {
            return type is CentralFeedTypeEnum.All
                ? _groupFeedService.GetFeed(groupId).OrderByDescending(item => item.PublishDate)
                : _groupFeedService.GetFeed(type, groupId);
        }

        public IEnumerable<IFeedItem> Sort(IEnumerable<IFeedItem> sortedItems, Enum type)
        {
            IEnumerable<IFeedItem> result;
            switch (type)
            {
                case CentralFeedTypeEnum.All:
                    result = sortedItems.OrderBy(i => i, new CentralFeedItemComparer());
                    break;
                default:
                    result = sortedItems.OrderByDescending(el => el.PublishDate);
                    break;
            }

            return result;
        }

        public string AvailableActivityTypes()
        {
            return _centralFeedService
                .GetAllSettings()
                .Where(s => !s.ExcludeFromAvailableActivityTypes)
                .Select(s => (Id: s.Type.ToInt(), Name: s.Type.ToString()))
                .Select(a => new { a.Id, a.Name })
                .OrderBy(el => el.Id)
                .ToJson();
        }
        public bool IsCentralFeedPage(IPublishedContent page)
        {
            return IsHomePage(page) || IsSubPage(page);
        }

        private bool IsHomePage(IPublishedContent page) =>
            _contentProvider.GetOverviewPage().Id == page.Id;

        private bool IsSubPage(IPublishedContent page) =>
            _contentProvider.GetRelatedPages().Any(c => c.IsAncestorOrSelf(page));

    }
}
