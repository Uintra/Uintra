using System.Collections.Generic;
using uCommunity.Events;
using uCommunity.Tagging;

namespace Compent.uIntra.Core.Events
{
    public class EventExtendedEditModel : EventEditModel, ITagsActivityCreateEditModel
    {
        public EventExtendedEditModel()
        {
            Tags = new List<TagEditModel>();
        }

        public IList<TagEditModel> Tags { get; set; }
    }
}