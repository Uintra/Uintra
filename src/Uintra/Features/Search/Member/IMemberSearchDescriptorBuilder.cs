using System;
using Nest;

namespace Uintra.Features.Search.Member
{
	public interface IMemberSearchDescriptorBuilder
	{
		QueryContainer[] GetMemberDescriptors(string query);
		QueryContainer GetMemberInGroupDescriptor(Guid? groupId);
	}
}