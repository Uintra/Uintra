using Compent.CommandBus;
using Uintra20.Core.Member.Abstractions;

namespace Uintra20.Core.Commands
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