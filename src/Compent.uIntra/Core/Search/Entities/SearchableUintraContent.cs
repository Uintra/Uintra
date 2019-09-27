﻿using System.Collections.Generic;
using Uintra.Search;

namespace Compent.Uintra.Core.Search.Entities
{
    public class SearchableUintraContent : SearchableContent,  ISearchableTaggedActivity
    {
        public IEnumerable<string> UserTagNames { get; set; }

        public bool TagsHighlighted { get; set; }
    }
}