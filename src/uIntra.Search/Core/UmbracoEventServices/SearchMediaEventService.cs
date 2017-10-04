using uIntra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Search
{
    public class SearchMediaEventService : IUmbracoMediaEventService
    {
        public void ProcessMediaSaved(IMediaService sender, SaveEventArgs<IMedia> args)
        {
            foreach (var media in args.SavedEntities)
            {
                //
            }
        }
    }
}
