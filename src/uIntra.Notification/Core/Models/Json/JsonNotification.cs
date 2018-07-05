using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Notification
{
    public class JsonNotification
    {
        public Guid Id { get; set; }
        public Guid ReceiverId { get; set; }
        public string Date { get; set; }
        public bool IsNotified { get; set; }
        public bool IsViewed { get; set; }
        public Enum Type { get; set; }
        public dynamic Value { get; set; }

        public Guid NotifierId { get; set; }
        public string NotifierPhoto { get; set; }
        public string NotifierDisplayedName { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
