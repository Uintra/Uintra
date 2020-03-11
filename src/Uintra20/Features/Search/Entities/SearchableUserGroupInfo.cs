using System;

namespace Uintra20.Features.Search.Entities
{
	public class SearchableMemberGroupInfo
	{
		public Guid GroupId { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsCreator { get; set; }
	}
}