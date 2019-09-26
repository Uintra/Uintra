using System.Collections.Generic;
using Compent.CommandBus;
using Uintra.Core.User;

namespace Uintra.Users.Commands
{
	public class MembersChanged : ICommand
	{
		public IEnumerable<IIntranetMember> Members { get; set; }

		public MembersChanged(IEnumerable<IIntranetMember> members)
		{
			Members = members;
		}
	}
}
