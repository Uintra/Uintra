using System.Collections.Generic;
using uIntra.Events;
using uIntra.Tagging;

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