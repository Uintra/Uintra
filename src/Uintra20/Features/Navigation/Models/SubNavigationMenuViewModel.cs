﻿using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Navigation.Models
{
    public class SubNavigationMenuViewModel
    {
        public string Title { get; set; }

        public bool IsTitleHidden { get; set; }

        public IEnumerable<SubNavigationMenuRowModel> Rows { get; set; } = Enumerable.Empty<SubNavigationMenuRowModel>();

        public MenuItemViewModel Parent { get; set; }

        public bool ShowBreadcrumbs { get; set; }
    }
}