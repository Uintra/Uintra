﻿using System.Collections.Generic;

namespace Uintra.Features.Groups.Models
{
    public class GroupLeftNavigationMenuViewModel
    {
        public GroupLeftNavigationItemViewModel GroupPageItem { get; set; }
        public IEnumerable<GroupLeftNavigationItemViewModel> Items { get; set; }
    }
}