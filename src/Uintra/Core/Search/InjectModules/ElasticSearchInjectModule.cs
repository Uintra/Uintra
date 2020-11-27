using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Search.Elasticsearch.SearchHighlighting;
using UBaseline.Search.Core;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Search.Indexers;
using Uintra.Core.Search.Repository;
using Uintra.Core.Search.Sorting;
using Uintra.Features.Events;
using Uintra.Features.News;
using Uintra.Features.Social;

namespace Uintra.Core.Search
{
    public class ElasticSearchInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddSingleton(typeof(IUintraSearchRepository<>), typeof(UintraSearchRepository<>));
            services.AddSingleton<IUintraSearchRepository, UintraSearchRepository>();
            services.AddSingleton<ISearchHighlightingHelper, UintraSearchHighlightingHelper>();

            services.AddScopedToCollection<ISearchDocumentIndexer, NewsService>();
            services.AddScopedToCollection<ISearchDocumentIndexer, ContentIndexer>();
            services.AddScopedToCollection<ISearchDocumentIndexer, EventsService>();
            services.AddScopedToCollection<ISearchDocumentIndexer, SocialService<Features.Social.Entities.Social>>();
            services.AddScopedToCollection<ISearchDocumentIndexer, DocumentIndexer>();
            services.AddScopedToCollection<ISearchDocumentIndexer, MemberIndexer>();
            services.AddScopedToCollection<ISearchDocumentIndexer, UserTagIndexer>();

            services.AddScopedToCollection<IDocumentIndexer, DocumentIndexer>();
            services.AddScoped<IContentIndexer, ContentIndexer>();
            services.AddScoped<IUserTagIndexer, UserTagIndexer>();

            services.AddScoped(typeof(ISearchSortingHelper<>), typeof(BaseSearchSortingHelper<>));


            return services;
        }
    }
}