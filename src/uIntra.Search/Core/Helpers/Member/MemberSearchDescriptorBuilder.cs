using System;
using System.Collections.Generic;
using System.Linq;
using Nest;

namespace Uintra.Search.Member
{
	public class MemberSearchDescriptorBuilder : IMemberSearchDescriptorBuilder
	{
		private readonly SearchScoreModel _scores;

		public MemberSearchDescriptorBuilder(ISearchScoreProvider searchScoreProvider)
		{
			_scores = searchScoreProvider.GetScores();
		}
		public virtual QueryContainer[] GetMemberDescriptors(string query)
		{
			var desc = new List<QueryContainer>
			{
				new QueryContainerDescriptor<SearchableMember>().Match(m => m
					.Query(query)
					.Analyzer(ElasticHelpers.Replace)
					.Field(f => f.Phone)
					.Boost(_scores.PhoneScore)),
				new QueryContainerDescriptor<SearchableMember>().Match(m => m
					.Query(query)
					.Analyzer(ElasticHelpers.Replace)
					.Field(f => f.Department)
					.Boost(_scores.DepartmentScore)),
				new QueryContainerDescriptor<SearchableMember>().Match(m => m
					.Query(query)
					.Analyzer(ElasticHelpers.Replace)
					.Field(f => f.FullName)
					.Boost(_scores.UserNameScore)),
				new QueryContainerDescriptor<SearchableMember>().Match(m => m
					.Query(query)
					.Analyzer(ElasticHelpers.Lowercase)
					.Field(f => f.Email)
					.Boost(_scores.UserEmailScore)),
			};
			return desc.ToArray();
		}

		public virtual QueryContainer GetMemberInGroupDescriptor(Guid? groupId)
		{
			return new QueryContainerDescriptor<SearchableMember>()
				.Nested(nst => nst
					.Path(p => p.Groups)
					.Query(q => q
						.Term(t => t
							.Field(f => f.Groups.First().GroupId)
							.Value(groupId.ToString()))));
		}
	}
}
