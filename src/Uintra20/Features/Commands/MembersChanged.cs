using System.Collections.Generic;
using Compent.CommandBus;
using Uintra20.Features.User;

namespace Uintra20.Features.Commands
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