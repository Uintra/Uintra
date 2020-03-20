using Uintra20.Features.Groups;

namespace Uintra20.Core.Search.Entities.Mappers
{
	public interface ISearchableMemberMapper<T> where T : SearchableMember
	{
		T Map(IGroupMember member);
	}
}
