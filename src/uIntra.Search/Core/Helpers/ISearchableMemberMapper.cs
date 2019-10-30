using Uintra.Groups;

namespace Uintra.Search
{
	public interface ISearchableMemberMapper 
	{
		SearchableMember Map(IGroupMember member);
	}
}
