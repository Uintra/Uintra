using System;
using Uintra.Core.User;

namespace Uintra.Notification
{
    public class NotifierViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string ProfileLink { get; set; }
        public UserViewModel User { get; set; }
    }
}