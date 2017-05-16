using System.Collections.Generic;
using uCommunity.Events;
using uCommunity.Tagging;

namespace Compent.uCommunity.Core.Events
{
    public class EventExtendedCreateModel : EventCreateModel, ITagsCreateEditModel
    {
        public IEnumerable<string> Tags { get; set; }
    }
}