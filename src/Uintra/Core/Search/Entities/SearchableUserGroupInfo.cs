using System;

namespace Uintra.Core.Search.Entities
{
	public class SearchableMemberGroupInfo
	{
		public Guid GroupId { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsCreator { get; set; }
	}
}