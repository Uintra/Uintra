using Compent.CommandBus;
using Uintra.Core.Member.Abstractions;

namespace Uintra.Core.Commands
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