using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Search.Contract;
using Compent.Shared.Search.Elasticsearch;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries.SearchByMember
{
    public class SearchByMemberSpecificationFactory : ISearchSpecificationFactory<SearchableMember, SearchByMemberQuery>
    {
        private readonly IDependencyProvider dependencyProvider;

        public SearchByMemberSpecificationFactory(IDependencyProvider dependencyProvider)
        {
            this.dependencyProvider = dependencyProvider;
        }

        public SearchQuerySpecification<SearchableMember> Create(SearchByMemberQuery query, string culture)
        {
            return new SearchByMemberSpecification(query, dependencyProvider, culture);
        }
    }
}