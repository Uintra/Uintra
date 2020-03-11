using System.Linq;
using Nest;

namespace Uintra20.Features.Search.Entities.Mappings
{
	public class SearchableUserMap : SearchableBaseMap<SearchableMember>
	{
		public SearchableUserMap()
		{
			Text(t => t.Name(n => n.Department).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram)
			.Fields(f => f
					.Keyword(k => k
						.Name(n => n.Department.Suffix(ElasticHelpers.Normalizer.Sort))
						.Normalizer(ElasticHelpers.Normalizer.Sort)
					)
			));
			Text(t => t.Name(n => n.Phone).Fielddata().Analyzer(ElasticHelpers.Phone));
			Text(t => t.Name(n => n.Email).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram));
			Text(t => t.Name(n => n.FullName).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram)
				.Fields(f => f
					.Keyword(k => k
						.Name(n => n.FullName.Suffix(ElasticHelpers.Normalizer.Sort))
						.Normalizer(ElasticHelpers.Normalizer.Sort)
					)
			));
			Text(t => t.Name(n => n.UserTagNames).Analyzer(ElasticHelpers.Tag));
			Nested<SearchableMemberGroupInfo>(nst =>
				nst.Name(n => n.Groups.First())
					.Properties(p =>
						p.Keyword(t => t.Name(n => n.GroupId))
							.Boolean(t => t.Name(n => n.IsAdmin))
								.Boolean(t => t.Name(n => n.IsCreator))));



		}
	}
}