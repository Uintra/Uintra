using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Entities;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Events;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uCommunity.Core.Events
{
    public class EventsService : IntranetActivityItemServiceBase<EventBase, Event>, IEventsService<EventBase, Event>, ICentralFeedItemService
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService _intranetUserService;

        public EventsService(UmbracoHelper umbracoHelper,
            IIntranetActivityService intranetActivityService,
            IMemoryCacheService memoryCacheService,
            IIntranetUserService intranetUserService)
            : base(intranetActivityService, memoryCacheService)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserService = intranetUserService;
        }

        protected override List<string> OverviewXPath { get; }
        public override IntranetActivityTypeEnum ActivityType => IntranetActivityTypeEnum.Events;

        public override IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContent(1163);
        }

        public override IPublishedContent GetDetailsPage()
        {
            return _umbracoHelper.TypedContent(1165);
        }

        public override IPublishedContent GetCreatePage()
        {
            return _umbracoHelper.TypedContent(1164);
        }

        public override IPublishedContent GetEditPage()
        {
            return _umbracoHelper.TypedContent(1166);
        }
      
        public IEnumerable<Event> GetPastEvents()
        {
            throw new NotImplementedException();
        }

        public void Hide(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool CanEditSubscribe(EventBase activity)
        {
            return true;
        }

        public bool CanSubscribe(EventBase activity)
        {
            return true;
        }

        public bool HasSubscribers(EventBase activity)
        {
            throw new NotImplementedException();
        }

        public MediaSettings GetMediaSettings()
        {
            return new MediaSettings
            {
                MediaRootId = 1099
            };
        }

        public CentralFeedSettings GetCentralFeedSettings()
        {
            return new CentralFeedSettings
            {
                Type = ActivityType,
                Controller = "Events",
                OverviewPage = GetOverviewPage(),
                CreatePage = GetCreatePage()
            };
        }

        public override bool CanEdit(EventBase activity)
        {
            return true;
        }

        public ICentralFeedItem GetItem(Guid activityId)
        {
            var item = Get(activityId);
            return item;
        }

        public IEnumerable<ICentralFeedItem> GetItems()
        {
            var items = GetManyActual().OrderByDescending(i => i.PublishDate);
            return items;
        }

        protected override Event FillPropertiesOnGet(IntranetActivityEntity entity)
        {
            var activity = base.FillPropertiesOnGet(entity);
            _intranetUserService.FillCreator(activity);

            return activity;
        }
    }
}