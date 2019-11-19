using Uintra.Groups;

namespace Uintra.Search
{
	public interface ISearchableMemberMapper<T> where T : SearchableMember
	{
		T Map(IGroupMember member);
	}
}
