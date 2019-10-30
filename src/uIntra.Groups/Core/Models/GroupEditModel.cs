using System;

namespace Uintra.Groups
{
    public class GroupEditModel : GroupCreateModel
    {
        public Guid Id { get; set; }
        public bool CanHide { get; set; }
    }
}