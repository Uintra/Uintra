using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Search.Contract;
using Compent.Shared.Search.Elasticsearch;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries
{
    public class SearchByTextSpecificationFactory : ISearchSpecificationFactory<SearchDocument, SearchByTextQuery>
    {
        private readonly IDependencyProvider dependencyProvider;

        public SearchByTextSpecificationFactory(IDependencyProvider dependencyProvider)
        {
            this.dependencyProvider = dependencyProvider;
        }

        public SearchQuerySpecification<SearchDocument> Create(SearchByTextQuery query, string culture)
        {
            var spec = new SearchByTextSpecification(query, dependencyProvider, culture);
            return spec;
        }
    }
}