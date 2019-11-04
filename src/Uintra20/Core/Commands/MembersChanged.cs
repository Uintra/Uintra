using System.Collections.Generic;
using Compent.CommandBus;
using Uintra20.Core.User;

namespace Uintra20.Core.Commands
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