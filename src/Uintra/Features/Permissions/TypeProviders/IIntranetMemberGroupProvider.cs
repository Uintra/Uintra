using System.Collections.Generic;
using Uintra.Features.Permissions.Models;

namespace Uintra.Features.Permissions.TypeProviders
{
    public interface IIntranetMemberGroupProvider
    {
        IntranetMemberGroup this[int typeId] { get; }

        IntranetMemberGroup this[string name] { get; }

        IEnumerable<IntranetMemberGroup> All { get; }

        IDictionary<int, IntranetMemberGroup> IntTypeDictionary { get; }

        IDictionary<string, IntranetMemberGroup> StringTypeDictionary { get; }
    }
}
