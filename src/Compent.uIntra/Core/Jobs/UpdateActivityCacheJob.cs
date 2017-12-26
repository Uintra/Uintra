using System;
using uIntra.Core.Activity;
using uIntra.Core.Jobs;
using uIntra.Events;
using uIntra.News;

namespace Compent.uIntra.Core.Jobs
{
    public class UpdateActivityCacheJob : BaseIntranetJob
    {
        private readonly INewsService<News.Entities.News> _newsService;
        private readonly IEventsService<EventBase> _eventsService;

        public UpdateActivityCacheJob(
            INewsService<News.Entities.News> newsService,
            IEventsService<EventBase> eventsService)
        {
            _newsService = newsService;
            _eventsService = eventsService;
        }

        public override JobSettings GetSettings()
        {
            return new JobSettings()
            {
                RunType = JobRunTypeEnum.RunEvery,
                TimeType = JobTimeType.Minutes,
                IsEnabled = true,
                Time = 1
            };
        }

        public override void Action()
        {
            ProcessEvents();
        }

        private void ProcessEvents()
        {
            var events = _eventsService.GetAll();
            var now = DateTime.Now;

            foreach (var @event in events)
            {
                if (@event.IsPinned && @event.EndPinDate.HasValue)
                {
                    if (@event.EndPinDate.Value <= now)
                    {
                        @event.IsPinned = false;
                        @event.IsPinActual = false;
                        @event.EndPinDate = null;

                        _eventsService.Save(@event);
                    }
                }
            }

        }

        private void ProcessNews()
        {
            var news = _newsService.GetAll();
            var now = DateTime.Now;

            foreach (var activity in news)
            {
                _newsService.P
            }
        }

        private void UnPinProccess(IntranetActivity activity)
        {
            
        }

        private bool NeedUnPin(IIntranetActivity activity)
    }
}