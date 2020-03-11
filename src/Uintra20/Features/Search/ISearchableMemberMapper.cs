using Uintra20.Features.Groups;
using Uintra20.Features.Search.Entities;

namespace Uintra20.Features.Search
{
	public interface ISearchableMemberMapper<T> where T : SearchableMember
	{
		T Map(IGroupMember member);
	}
}
