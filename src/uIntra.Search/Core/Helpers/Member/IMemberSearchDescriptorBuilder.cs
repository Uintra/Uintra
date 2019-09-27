using System;
using LanguageExt;
using Nest;

namespace Uintra.Search.Member
{
	public interface IMemberSearchDescriptorBuilder
	{
		QueryContainer[] GetMemberDescriptors(string query);
		QueryContainer GetMemberInGroupDescriptor(Guid? groupId);
	}
}