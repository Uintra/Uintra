using System;
using System.Linq;
using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static Uintra.Core.Constants.UmbracoAliases.Media;

namespace Uintra.Search
{
    public class SearchMediaEventService : IUmbracoMediaSavedEventService, IUmbracoMediaTrashedEventService, IUmbracoMediaSavingEventService
    {
        private readonly IDocumentIndexer _documentIndexer;

        public SearchMediaEventService(IDocumentIndexer documentIndexer)
        {
            _documentIndexer = documentIndexer;
        }

        public void ProcessMediaSaving(IMediaService sender, SaveEventArgs<IMedia> args)
        {
            foreach (var media in args.SavedEntities.Where(i => !i.HasIdentity))
                media.CreateDate = DateTime.UtcNow;
        }

        public void ProcessMediaSaved(IMediaService sender, SaveEventArgs<IMedia> args)
        {
            var actualMedia = args
                .SavedEntities
                .Where(m => !m.IsNewEntity());

            foreach (var media in actualMedia)
            {
                if (IsAllowedForSearch(media))
                    _documentIndexer.Index(media.Id);
                else _documentIndexer.DeleteFromIndex(media.Id);
            }
        }

        public void ProcessMediaTrashed(IMediaService sender, MoveEventArgs<IMedia> args)
        {
            args.MoveInfoCollection
                .Select(i => i.Entity.Id)
                .ToList()
                .ForEach(_documentIndexer.DeleteFromIndex);
        }

        private bool IsAllowedForSearch(IMedia media)
        {
            return media.HasProperty(UseInSearchPropertyAlias) &&
                   media.GetValue<bool>(UseInSearchPropertyAlias);
        }
    }
}
