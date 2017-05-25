using System.Collections.Generic;
using uIntra.Events;
using uIntra.Tagging;

namespace Compent.uIntra.Core.Events
{
    public class EventExtendedCreateModel : EventCreateModel, ITagsActivityCreateEditModel
    {
        public EventExtendedCreateModel()
        {
            Tags = new List<TagEditModel>();
        }

        public IList<TagEditModel> Tags { get; set; }
    }
}