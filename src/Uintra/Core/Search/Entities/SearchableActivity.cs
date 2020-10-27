﻿using System;

namespace Uintra.Core.Search.Entities
{
    public class SearchableActivity : SearchableBase
    {
        public string Description { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsPinned { get; set; }

        public bool IsPinActual { get; set; }
    }
}