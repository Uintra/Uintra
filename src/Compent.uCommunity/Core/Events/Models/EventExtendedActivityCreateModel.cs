using System.Collections.Generic;
using uCommunity.Events;
using uCommunity.Tagging;

namespace Compent.uCommunity.Core.Events
{
    public class EventExtendedActivityCreateModel : EventCreateModel, ITagsActivityCreateEditModel
    {
        public EventExtendedActivityCreateModel()
        {
            Tags = new List<TagEditModel>();
        }

        public IList<TagEditModel> Tags { get; set; }
    }
}