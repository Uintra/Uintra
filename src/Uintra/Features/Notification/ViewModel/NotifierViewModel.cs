using System;
using Uintra.Core.Member.Models;

namespace Uintra.Features.Notification.ViewModel
{
    public class NotifierViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string ProfileLink { get; set; }
        public MemberViewModel Member { get; set; }
    }
}