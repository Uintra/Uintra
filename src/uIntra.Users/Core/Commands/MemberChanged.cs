using Compent.CommandBus;
using Uintra.Core.User;

namespace Uintra.Users.Commands
{
	public class MemberChanged : ICommand
	{
		public IIntranetMember Member { get; set; }

		public MemberChanged(IIntranetMember member)
		{
			Member = member;
		}
	}
}
