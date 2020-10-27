using Uintra.Features.Groups;

namespace Uintra.Core.Search.Entities.Mappers
{
	public interface ISearchableMemberMapper<T> where T : SearchableMember
	{
		T Map(IGroupMember member);
	}
}
