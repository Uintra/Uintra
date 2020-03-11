using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Search.Entities
{
	public class SearchableMember : SearchableBase, ISearchableTaggedActivity
	{
		public string Photo { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public string Department { get; set; }

		public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();

		public bool TagsHighlighted { get; set; }

		public bool Inactive { get; set; }

		public IEnumerable<SearchableMemberGroupInfo> Groups { get; set; } = Enumerable.Empty<SearchableMemberGroupInfo>();
	}
}
