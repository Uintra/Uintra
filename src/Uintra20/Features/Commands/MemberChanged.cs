using Compent.CommandBus;
using Uintra20.Features.User;

namespace Uintra20.Features.Commands
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