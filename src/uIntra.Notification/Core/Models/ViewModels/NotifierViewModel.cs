using System;
using uIntra.Core.User;

namespace uIntra.Notification
{
    public class NotifierViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string ProfileLink { get; set; }
        public IIntranetUser User { get; set; }
    }
}