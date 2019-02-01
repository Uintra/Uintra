using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Search;

namespace Compent.Uintra.Core.Search.Entities
{
    public class SearchableUser : SearchableBase, ISearchibleTaggedActivity
    {
        public string Photo { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Department { get; set; }

        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();

        public bool TagsHighlighted { get; set; }

        public bool Inactive { get; set; }

        public IEnumerable<Guid> GroupIds { get; set; } = Enumerable.Empty<Guid>();
    }
}